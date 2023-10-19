using Advancly.Core.DTOs;
using Advancly.Core.Interface;
using Advancly.Domain.Entitities;
using Advancly.Infrastructure;
using Advancly.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Advancly.Core.Services
{
    public class AccountService : AdvanclyRepository<AdvanclyUser>, IAccountService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(AdvanclyDbContext context, ITransactionService transactionService, ILogger<AccountService> logger) : base(context)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        public async Task<AdvanclyUser> GetAccountByUserId(string userId)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<AccountDTO> GetAccountByUserIdandBVN(string userId, string BVN)
        {
            var account = await _dbSet.FirstOrDefaultAsync(a => a.Id == userId);

            if (account != null)
            {
                if (account.BVN == BVN)
                {
                    return new AccountDTO
                    {
                        AccountNumber = account.AccountNumber,
                        AccountName = account.FirstName + " " + account.LastName,
                        Balance = account.Balance
                    };
                }
            }
            return new AccountDTO();
        }

        public async Task<ResponseDTO<bool>> Transfer(Domain.Entitities.Transaction transaction)
        {
            transaction.TimeStamp = DateTime.UtcNow;
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existingTransaction = await _transactionService.GetAsync(v => v.RequestID == transaction.RequestID);
                if (existingTransaction != null)
                {
                    return new ResponseDTO<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = "This transaction has already been processed."
                    };
                }
                var senderAccount = await _dbSet.FirstOrDefaultAsync(a => a.AccountNumber == transaction.SourceAccount);
                var receiverAccount = await _dbSet.FirstOrDefaultAsync(a => a.AccountNumber == transaction.DestAccount);
                if (transaction.SourceAccount == transaction.DestAccount)
                {
                    return new ResponseDTO<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = "Source and Destination Account cannot be the same"
                    };
                }


                if (senderAccount.Balance >= transaction.Amount)
                {
                    senderAccount.Balance -= transaction.Amount;
                    receiverAccount.Balance += transaction.Amount;

                    await _transactionService.AddAsync(transaction);
                    await _context.SaveChangesAsync();
                    scope.Complete();
                    return new ResponseDTO<bool>
                    {
                        Success = true,
                        Data = true,
                        Message = "Transaction Successful"
                    };
                }

                return new ResponseDTO<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Insufficient Balance"
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseDTO<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "A concurrency conflict occurred. Please try again."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseDTO<bool> { Success = false, Data = false, Message = $"An error occured, please try again later." };
            }
        }

        public async Task<ResponseDTO<bool>> Deposit(string accountNumber, decimal amount, string requestId)
        {
            try
            {
                var existingTransaction = await _transactionService.GetAsync(v => v.RequestID == requestId);
                if (existingTransaction != null)
                {
                    return new ResponseDTO<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = "This transaction has already been processed."
                    };
                }
                var account = await _dbSet.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                if (account == null)
                {
                    return new ResponseDTO<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = "Account not found."
                    };
                }

                account.Balance += amount;
                var transaction = new Domain.Entitities.Transaction
                {
                    Amount = amount,
                    DestAccount = accountNumber,
                    SourceAccount = accountNumber,
                    Narration = "Deposit",
                    RequestID = requestId,
                    TimeStamp = DateTime.UtcNow,
                    RowVersion = 1
                };
                await _transactionService.AddAsync(transaction);
                await _context.SaveChangesAsync();

                return new ResponseDTO<bool>
                {
                    Success = true,
                    Data = true,
                    Message = $"Credited {amount} to {accountNumber}. New Balance: {account.Balance}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseDTO<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "An error occurred. Please try again later."
                };
            }
        }
    }
}
