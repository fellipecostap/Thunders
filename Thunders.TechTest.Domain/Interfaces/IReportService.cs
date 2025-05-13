using Thunders.TechTest.Domain.Entities;

namespace Thunders.TechTest.Domain.Interfaces;

public interface IReportService
{
    Task<ReportEntity> GenerateHourlyByCityReport(DateTime date);
    Task<ReportEntity> GenerateTopPlazasReport(int month, int year, int top);
    Task<ReportEntity> GenerateVehicleCountReport(string plazaId, DateTime? startDate = null, DateTime? endDate = null);
    Task<ReportEntity?> GetReportAsync(Guid id);
}
