using MediatR;
using Thunders.TechTest.Domain.Enums;

namespace Thunders.TechTest.Application.Services.Reports.GenerateHourly;

public record GetReportByIdRequest : IRequest<ReportDto>
{
    public Guid? Id { get; set; }
}