using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Transactions.Queries.GetTransactionById;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Budgetly.Domain.ValueObjects;

namespace Budgetly.Application.Tests.Transactions.Queries;

public sealed class GetTransactionByIdHandlerTests
{
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();

    private GetTransactionByIdHandler CreateSut() => new(_transactionRepository);

    [Fact]
    public async Task Handle_TransactionExists_ReturnsDto()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(100m, "USD"),
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            null);

        _transactionRepository
            .GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);

        var result = await CreateSut().Handle(new GetTransactionByIdQuery(transaction.Id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(transaction.Id);
    }

    [Fact]
    public async Task Handle_TransactionNotFound_ReturnsNotFoundError()
    {
        var missingId = Guid.NewGuid();

        _transactionRepository
            .GetByIdAsync(missingId, Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        var result = await CreateSut().Handle(new GetTransactionByIdQuery(missingId), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldEndWith(".NotFound");
    }
}
