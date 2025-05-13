// ApiService/Controllers/TransactionsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Commands;
using Thunders.TechTest.Application.Services.TollTransaction.Create;
using Thunders.TechTest.Domain.Entities;

namespace Thunders.TechTest.ApiService.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTollTransactionRequest command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var transactionId = await _mediator.Send(command);
        return Accepted(transactionId);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetBatch([FromQuery] int count)
    {
        var result = await _mediator.Send(new GetTollTransactionRequest() { Count = count });
        return Ok(result);
    }
}