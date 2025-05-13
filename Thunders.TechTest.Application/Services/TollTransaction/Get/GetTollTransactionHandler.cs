using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Commands;
using Thunders.TechTest.Application.Services.TollTransaction.Get;
using Thunders.TechTest.OutOfBox.Database;

namespace Thunders.TechTest.ApiService.Handlers;

public class GetTransactionsBatchHandler : IRequestHandler<GetTollTransactionRequest, IEnumerable<TollTransactionDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetTransactionsBatchHandler(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TollTransactionDto>> Handle(GetTollTransactionRequest request, CancellationToken cancellationToken)
    {
        var getTollTransactions = await _dbContext.TollTransaction
            .OrderByDescending(t => t.DateTime)
            .Take(request.Count ?? 0)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IList<TollTransactionDto>>(request);
    }
}