using Budgetly.Application.Accounts.Commands.CreateAccount;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;

namespace Budgetly.Application.Tests.Accounts.Commands;

public sealed class CreateAccountHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CreateAccountHandler CreateSut() => new(_accountRepository, _unitOfWork);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessWithNewGuidAndPersistsWithZeroBalance()
    {
        var command = new CreateAccountCommand("Checking", "USD");

        var result = await CreateSut().Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);

        await _accountRepository.Received(1).AddAsync(
            Arg.Is<Account>(a => a.Name == "Checking" && a.Balance.Amount == 0m && a.Balance.Currency == "USD"),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
