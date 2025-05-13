// ApiService/Controllers/ReportsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.Application.Services.Reports.GenerateHourly;

namespace Thunders.TechTest.ApiService.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("hourly-by-city")]
    public async Task<IActionResult> GenerateHourlyByCity([FromBody] GenerateHourlyByCityRequest query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReport(Guid id)
    {
        var result = await _mediator.Send(new GetReportByIdRequest { Id = id});
        return Ok(result);
    }
}