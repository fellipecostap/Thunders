using Thunders.TechTest.Domain.Enums;

namespace Thunders.TechTest.Domain.Entities
{
    public class ReportEntity
    {
        public Guid Id { get; set; }
        public ReportTypeEnum ReportType { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string Parameters { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty; // JSON serialized report data
    }
}
