using Budgetly.Application.Accounts.Queries.GetAccountById;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;

namespace Budgetly.Application.Tests.Accounts.Queries;

public sealed class GetAccountByIdHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

    private GetAccountByIdHandler CreateSut() => new(_accountRepository);

    [Fact]
    public async Task Handle_AccountExists_ReturnsDto()
    {
        var account = Account.Create("Checking", "USD");

        _accountRepository
            .GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var result = await CreateSut().Handle(new GetAccountByIdQuery(account.Id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(account.Id);
        result.Value.Name.ShouldBe("Checking");
    }

    [Fact]
    public async Task Handle_AccountNotFound_ReturnsNotFoundError()
    {
        var missingId = Guid.NewGuid();

        _accountRepository
            .GetByIdAsync(missingId, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        var result = await CreateSut().Handle(new GetAccountByIdQuery(missingId), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldEndWith(".NotFound");
    }
}
