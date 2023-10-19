using Advancly.Core.AppSettings;
using Advancly.Core.DTOs;
using Advancly.Domain.Entitities;
using Advancly.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Advancly.Controllers
{
    /// <summary>
    /// Accounts controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly AdvanclyDbContext _context;
        private readonly Jwt _jwt;

        #endregion

        #region ctor
        /// <summary>
        /// Accounts constructor.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="provider"></param>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser>
            signInManager, IServiceProvider provider, IMapper mapper, AdvanclyDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _jwt = provider.GetRequiredService<Jwt>();

        }
        #endregion

        #region Methods
        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="creds"></param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        public async Task<ActionResult<AuthenticationResponse>> CreateUserAsync([FromBody] UserCredentials creds)
        {
            var user = new AdvanclyUser
            {
                UserName = creds.Email.Split('@')[0],
                Email = creds.Email,
                FirstName = creds.FirstName,
                LastName = creds.LastName,
                BVN = creds.BVN,
                DateOfBirth = creds.DateOfBirth,
                Address = creds.Address,
                PhoneNumber = creds.PhoneNumber,
                AccountNumber = GenerateAccountNumber(),
            };
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                return await BuildToken(user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="creds"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginCredentials creds)
        {
            var result = await _signInManager.PasswordSignInAsync(creds.Email.Split('@')[0], creds.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(creds.Email) as AdvanclyUser;
                return await BuildToken(user);
            }
            else
            {
                return BadRequest("Invalid login attempt!");
            }
        }

        private string GenerateAccountNumber()
        {
            Random rnd = new Random();

            int firstDigit = rnd.Next(0, 10);

            string remainingDigits = string.Concat(Enumerable.Range(0, 9).Select(n => rnd.Next(0, 10).ToString()));

            return firstDigit.ToString() + remainingDigits;
        }

        private async Task<AuthenticationResponse> BuildToken(AdvanclyUser user)
        {
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.NameIdentifier,  user.AccountNumber),
                    };
            var advanclyUser = await _userManager.FindByNameAsync(user.Email.Split('@')[0]);
            var claimsDb = await _userManager.GetClaimsAsync(advanclyUser);
            claims.AddRange(claimsDb);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Token));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);
            var token = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                );
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
        #endregion
    }
}

