// ApiService/Handlers/CreateTransactionHandler.cs
using MediatR;
using Thunders.TechTest.Domain.Entities;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.Application.Services.TollTransaction.Create;

public class CreateTransactionHandler : IRequestHandler<CreateTollTransactionRequest, Guid>
{
    private readonly IMessageSender _messageSender;
    private readonly AppDbContext _dbContext;

    public CreateTransactionHandler(IMessageSender messageSender, AppDbContext dbContext)
    {
        _messageSender = messageSender;
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateTollTransactionRequest command, CancellationToken cancellationToken)
    {
        var transaction = new TollTransactionEntity
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.Now,
            PlazaId = command.PlazaId,
            PlazaName = command.PlazaName,
            City = command.City,
            State = command.State,
            Amount = command.Amount ?? 0,
            VehicleType = command.VehicleType ?? 0
        };

        _dbContext.TollTransaction.Add(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _messageSender.Publish(transaction);

        return transaction.Id;
    }
}