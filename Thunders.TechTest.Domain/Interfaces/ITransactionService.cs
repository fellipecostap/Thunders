using Thunders.TechTest.Domain.Entities;

namespace Thunders.TechTest.Domain.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TollTransactionEntity>> GetBatchAsync(int count);
}

