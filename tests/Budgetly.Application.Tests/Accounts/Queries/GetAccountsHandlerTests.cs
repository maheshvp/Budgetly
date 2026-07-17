using Budgetly.Application.Accounts.Queries.GetAccounts;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;

namespace Budgetly.Application.Tests.Accounts.Queries;

public sealed class GetAccountsHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

    private GetAccountsHandler CreateSut() => new(_accountRepository);

    [Fact]
    public async Task Handle_AccountsExist_ReturnsMappedDtos()
    {
        var account = Account.Create("Checking", "USD");

        _accountRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns([account]);

        var result = await CreateSut().Handle(new GetAccountsQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(1);
        result.Value[0].Id.ShouldBe(account.Id);
        result.Value[0].Name.ShouldBe("Checking");
        result.Value[0].Balance.ShouldBe(0m);
        result.Value[0].Currency.ShouldBe("USD");
    }

    [Fact]
    public async Task Handle_NoAccounts_ReturnsEmptyList()
    {
        _accountRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await CreateSut().Handle(new GetAccountsQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEmpty();
    }
}
