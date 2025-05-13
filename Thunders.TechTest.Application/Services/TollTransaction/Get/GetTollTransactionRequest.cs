using MediatR;
using Thunders.TechTest.Application.Services.TollTransaction.Get;

namespace Thunders.TechTest.ApiService.Commands;

public record GetTollTransactionRequest : IRequest<IEnumerable<TollTransactionDto>>
{
    public int? Count { get; set; }
}