using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.Domain.Enums;
using Thunders.TechTest.OutOfBox.Database;

namespace Thunders.TechTest.Application.Services.Reports.GenerateHourly;

public class GenerateHourlyByCityHandler : IRequestHandler<GenerateHourlyByCityRequest, ReportDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GenerateHourlyByCityHandler(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ReportDto> Handle(GenerateHourlyByCityRequest query, CancellationToken cancellationToken)
    {
        var transactions = await _dbContext.TollTransaction
            .Where(t => t.DateTime.Date == query.DateTime.Value.Date)
            .ToListAsync(cancellationToken);

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
            Parameters = JsonSerializer.Serialize(new { query.DateTime }),
            Data = JsonSerializer.Serialize(result)
        };

        _dbContext.Report.Add(report);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReportDto>(report);
    }
}