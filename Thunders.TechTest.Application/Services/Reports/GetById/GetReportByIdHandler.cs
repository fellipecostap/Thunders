using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.Domain.Enums;
using Thunders.TechTest.OutOfBox.Database;

namespace Thunders.TechTest.Application.Services.Reports.GenerateHourly;

public class GetReportByIdHandler : IRequestHandler<GetReportByIdRequest, ReportDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetReportByIdHandler(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ReportDto> Handle(GetReportByIdRequest query, CancellationToken cancellationToken)
    {
        var report = await _dbContext.Report
            .Where(t => t.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<ReportDto>(report);
    }
}