using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Transactions.Queries.GetTransactions;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Budgetly.Domain.ValueObjects;

namespace Budgetly.Application.Tests.Transactions.Queries;

public sealed class GetTransactionsHandlerTests
{
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();

    private GetTransactionsHandler CreateSut() => new(_transactionRepository);

    [Fact]
    public async Task Handle_NoFilters_ReturnsAllMappedDtos()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(100m, "USD"),
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            null);

        _transactionRepository
            .GetAllAsync(null, null, null, Arg.Any<CancellationToken>())
            .Returns([transaction]);

        var result = await CreateSut().Handle(new GetTransactionsQuery(null, null, null), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(1);
        result.Value[0].Id.ShouldBe(transaction.Id);
        result.Value[0].Amount.ShouldBe(100m);
        result.Value[0].Currency.ShouldBe("USD");
    }

    [Fact]
    public async Task Handle_WithAccountIdFilter_PassesFilterThroughToRepository()
    {
        var accountId = Guid.NewGuid();
        var from = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));
        var to = DateOnly.FromDateTime(DateTime.UtcNow);

        _transactionRepository
            .GetAllAsync(accountId, from, to, Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await CreateSut().Handle(
            new GetTransactionsQuery(accountId, from, to), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        await _transactionRepository.Received(1)
            .GetAllAsync(accountId, from, to, Arg.Any<CancellationToken>());
    }
}
