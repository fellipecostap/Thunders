using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.Domain.Interfaces;
using Thunders.TechTest.OutOfBox.Database;

namespace Thunders.TechTest.ServiceDefaults.Repository
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _dbContext;

        public TransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TollTransactionEntity>> GetBatchAsync(int count)
        {
            return await _dbContext.TollTransaction
                .OrderByDescending(t => t.DateTime)
                .Take(count)
                .ToListAsync();
        }
    }
}
