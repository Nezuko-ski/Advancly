using Advancly.Core.DTOs;
using Advancly.Domain.Entitities;
using Advancly.Infrastructure.Repository;

namespace Advancly.Core.Interface
{
    public interface IAccountService : IAdvanclyRepository<AdvanclyUser>
    {
        Task<AdvanclyUser> GetAccountByUserId(string userId);
        Task<AccountDTO> GetAccountByUserIdandBVN(string userId, string BVN);
        Task<ResponseDTO<bool>> Transfer(Transaction transaction);
        Task<ResponseDTO<bool>> Deposit(string accountNumber, decimal amount, string requestId);
    }
}