using Advancly.Domain.Entitities;
using Advancly.Infrastructure.Repository;

namespace Advancly.Core.Interface
{
    public interface ITransactionService : IAdvanclyRepository<Transaction>
    {
    }
}
