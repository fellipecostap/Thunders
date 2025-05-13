using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.Domain.Enums;
using Thunders.TechTest.Domain.Interfaces;
using Thunders.TechTest.OutOfBox.Database;

namespace Thunders.TechTest.Application
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _dbContext;

        public ReportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReportEntity> GenerateHourlyByCityReport(DateTime date)
        {
            var transactions = await _dbContext.TollTransaction
                .Where(t => t.DateTime.Date == date.Date)
                .ToListAsync();

            var result = transactions
                .GroupBy(t => new { t.City, t.DateTime.Hour })
                .Select(g => new
                {
                    g.Key.City,
                    Hour = $"{g.Key.Hour}:00",
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .GroupBy(r => r.City)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(r => r.Hour, r => r.TotalAmount)
                );

            var report = new ReportEntity
            {
                Id = Guid.NewGuid(),
                ReportType = ReportTypeEnum.HourlyByCity,
                GeneratedAt = DateTime.UtcNow,
                Parameters = JsonSerializer.Serialize(new { Date = date }),
                Data = JsonSerializer.Serialize(result)
            };

            _dbContext.Report.Add(report);
            await _dbContext.SaveChangesAsync();

            return report;
        }

        public async Task<ReportEntity> GenerateTopPlazasReport(int month, int year, int top)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var transactions = await _dbContext.TollTransaction
                .Where(t => t.DateTime >= startDate && t.DateTime <= endDate)
                .ToListAsync();

            var result = transactions
                .GroupBy(t => t.PlazaId)
                .Select(g => new
                {
                    PlazaId = g.Key,
                    g.First().PlazaName,
                    TotalAmount = g.Sum(t => t.Amount),
                    TransactionCount = g.Count()
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(top)
                .ToList();

            var report = new ReportEntity
            {
                Id = Guid.NewGuid(),
                ReportType = ReportTypeEnum.TopPlazas,
                GeneratedAt = DateTime.UtcNow,
                Parameters = JsonSerializer.Serialize(new { Month = month, Year = year, Top = top }),
                Data = JsonSerializer.Serialize(result)
            };

            _dbContext.Report.Add(report);
            await _dbContext.SaveChangesAsync();

            return report;
        }

        public async Task<ReportEntity> GenerateVehicleCountReport(string plazaId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbContext.TollTransaction
                .Where(t => t.PlazaId == plazaId);

            if (startDate.HasValue)
                query = query.Where(t => t.DateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.DateTime <= endDate.Value);

            var transactions = await query.ToListAsync();

            var result = transactions
                .GroupBy(t => t.VehicleType)
                .Select(g => new
                {
                    VehicleType = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToDictionary(x => x.VehicleType, x => x.Count);

            var report = new ReportEntity
            {
                Id = Guid.NewGuid(),
                ReportType = ReportTypeEnum.VehicleCount,
                GeneratedAt = DateTime.UtcNow,
                Parameters = JsonSerializer.Serialize(new { PlazaId = plazaId, StartDate = startDate, EndDate = endDate }),
                Data = JsonSerializer.Serialize(result)
            };

            _dbContext.Report.Add(report);
            await _dbContext.SaveChangesAsync();

            return report;
        }

        public async Task<ReportEntity?> GetReportAsync(Guid id)
        {
            return await _dbContext.Report.FindAsync(id);
        }
    }
}