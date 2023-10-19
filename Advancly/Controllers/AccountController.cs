using Advancly.Core.DTOs;
using Advancly.Core.Interface;
using Advancly.Domain.Entitities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Advancly.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        #region Fields
        private readonly IAccountService _accountService;
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountService"></param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="BVN"></param>
        /// <returns></returns>
        [HttpGet("GetAccountDetails")]
        public async Task<IActionResult> GetAccountByUserIdandBVN(string userId, string BVN)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(BVN))
            {
                return BadRequest();
            }
            var account = await _accountService.GetAccountByUserIdandBVN(userId, BVN);
            return Ok(account);
        }

        /// <summary>
        /// Send funds to another account
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(Transaction transaction)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAccount = await _accountService.GetAccountByUserId(userId);
            if (userAccount == null || userAccount.AccountNumber != transaction.SourceAccount)
            {
                return BadRequest(new ResponseDTO<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "You are not authorized to initiate a transfer from this account."
                });
            }
            var transferResponse = await _accountService.Transfer(transaction);
            return Ok(transferResponse);
        }

        /// <summary>
        /// Deposit funds
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        public async Task<IActionResult> DepositAccount([FromBody] CreditRequest request)
        {
            var result = await _accountService.Deposit(request.AccountNumber, request.Amount, request.RequestId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        #endregion
    }
}
