using Thunders.TechTest.Domain.Enums;

namespace Thunders.TechTest.Domain.Entities;

public class TollTransactionEntity
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string PlazaId { get; set; } = string.Empty;
    public string PlazaName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public VehicleTypeEnum VehicleType { get; set; }
}

