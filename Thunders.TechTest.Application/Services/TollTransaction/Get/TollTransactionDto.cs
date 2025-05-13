using Thunders.TechTest.Application.Mappings;
using Thunders.TechTest.Domain.Entities;

namespace Thunders.TechTest.Application.Services.TollTransaction.Get;

public class TollTransactionDto : IMapFrom<TollTransactionEntity>
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
}
