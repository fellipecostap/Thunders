using MediatR;
using Thunders.TechTest.Domain.Enums;

namespace Thunders.TechTest.Application.Services.Reports.GenerateHourly;

public record GenerateHourlyByCityRequest : IRequest<ReportDto>
{
    public DateTime? DateTime { get; set; }
    public string? PlazaId { get; set; }
    public string? PlazaName { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public decimal? Amount { get; set; }
    public VehicleTypeEnum? VehicleType { get; set; }
}