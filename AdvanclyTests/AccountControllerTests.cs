using Advancly.Controllers;
using Advancly.Core.DTOs;
using Advancly.Core.Interface;
using Advancly.Domain.Entitities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace AdvanclyTests
{
    public class AccountControllerTests
    {
        #region Fields
        private readonly AccountController _controller;
        private readonly Mock<IAccountService> _mockAccountService;
        #endregion

        #region ctor
        public AccountControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountController(_mockAccountService.Object);
        }
        #endregion

        #region Methods
        [Fact]
        public async Task GetAccountByUserIdandBVN_EmptyParams_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetAccountByUserIdandBVN("", "");

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetAccountByUserIdandBVN_ValidParams_ReturnsOkResult()
        {
            // Arrange
            var expectedAccount = new AccountDTO();
            _mockAccountService.Setup(x => x.GetAccountByUserIdandBVN(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedAccount);

            // Act
            var result = await _controller.GetAccountByUserIdandBVN("sampleUserId", "sampleBVN");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedAccount, okResult.Value);
        }


        [Fact]
        public async Task GetAccountByUserIdandBVN_SpecificParams_ReturnsSpecificAccount()
        {
            // Arrange
            string testUserId = "f29ba76a-e241-4b15-9489-3254889985d7";
            string testBVN = "56072575268";

            var expectedAccount = new AccountDTO()
            {
                AccountName = "Uche Montana",
                AccountNumber = "0034818902",
                Balance = 1000
            };

            _mockAccountService.Setup(x => x.GetAccountByUserIdandBVN(testUserId, testBVN)).ReturnsAsync(expectedAccount);

            // Act
            var result = await _controller.GetAccountByUserIdandBVN(testUserId, testBVN);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccount = okResult.Value as AccountDTO;

            Assert.NotNull(returnedAccount);
            Assert.Equal(expectedAccount.AccountName, returnedAccount.AccountName);
            Assert.Equal(expectedAccount.AccountNumber, returnedAccount.AccountNumber);
            Assert.Equal(expectedAccount.Balance, returnedAccount.Balance);
        }

        [Fact]
        public async Task Transfer_UnauthorizedTransfer_ReturnsBadRequest()
        {
            // Arrange
            var transaction = new Transaction
            {
                SourceAccount = "0123456789"
            };

            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, "testUserId");
            var identity = new ClaimsIdentity(new[] { userIdClaim }, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };

            _mockAccountService.Setup(x => x.GetAccountByUserId(It.IsAny<string>())).ReturnsAsync(new AdvanclyUser { AccountNumber = "9876543210" });

            // Act
            var result = await _controller.Transfer(transaction);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Transfer_ValidTransfer_ReturnsOkResult()
        {
            // Arrange
            var transaction = new Transaction
            {
                SourceAccount = "2048634213",
                DestAccount = "2045686533",
                Amount = 1000,
                RequestID = "JACKSON09",
                TimeStamp = DateTime.UtcNow,
                RowVersion = 1,
                Narration = "Test Transfer"
            };
            var transferResponse = new ResponseDTO<bool>()
            {
                Success = true,
                Data = true,
                Message = "Transaction Successful"
            };
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, "testUserId");
            var identity = new ClaimsIdentity(new[] { userIdClaim }, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
            _mockAccountService.Setup(x => x.GetAccountByUserId(It.IsAny<string>())).ReturnsAsync(new AdvanclyUser { AccountNumber = "2048634213" });
            _mockAccountService.Setup(x => x.Transfer(transaction)).ReturnsAsync(transferResponse);

            // Act
            var result = await _controller.Transfer(transaction);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(transferResponse, okResult.Value);
        }

        [Fact]
        public async Task DepositAccount_ValidDeposit_ReturnsOkResult()
        {
            // Arrange
            var creditRequest = new CreditRequest
            {
                AccountNumber = "2048634213",
                Amount = 1000,
                RequestId = "NKUNKU08"
            };
            var depositResponse = new ResponseDTO<bool>()
            {
                Success = true,
                Data = true,
                Message = $"Credited {creditRequest.Amount} to {creditRequest.AccountNumber}"
            };
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, "testUserId");
            var identity = new ClaimsIdentity(new[] { userIdClaim }, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
            _mockAccountService.Setup(x => x.Deposit(creditRequest.AccountNumber, creditRequest.Amount, creditRequest.RequestId)).ReturnsAsync(depositResponse);

            // Act
            var result = await _controller.DepositAccount(creditRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(depositResponse, okResult.Value);
        }
        #endregion
    }
}
