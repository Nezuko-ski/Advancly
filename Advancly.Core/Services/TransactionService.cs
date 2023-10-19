using Advancly.Core.Interface;
using Advancly.Domain.Entitities;
using Advancly.Infrastructure;
using Advancly.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace Advancly.Core.Services
{
    public class TransactionService : AdvanclyRepository<Transaction>, ITransactionService
    {
        #region Fields
        private readonly ILogger<TransactionService> _logger;
        #endregion

        #region ctor
        public TransactionService(AdvanclyDbContext context, ILogger<TransactionService> logger) : base(context)
        {
            _logger = logger;
        }
        #endregion

    }
}
