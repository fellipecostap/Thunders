using Thunders.TechTest.Application.Mappings;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.Domain.Enums;

namespace Thunders.TechTest.Application.Services.Reports.GenerateHourly;

public class ReportDto : IMapFrom<ReportEntity>
{
    public Guid Id { get; set; }
    public ReportTypeEnum ReportType { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string Parameters { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}
