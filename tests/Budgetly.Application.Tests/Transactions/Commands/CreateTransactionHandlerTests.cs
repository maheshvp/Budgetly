using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Transactions.Commands.CreateTransaction;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;

namespace Budgetly.Application.Tests.Transactions.Commands;

public sealed class CreateTransactionHandlerTests
{
    private readonly IAccountRepository     _accountRepository     = Substitute.For<IAccountRepository>();
    private readonly ICategoryRepository    _categoryRepository    = Substitute.For<ICategoryRepository>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly IUnitOfWork            _unitOfWork            = Substitute.For<IUnitOfWork>();

    private CreateTransactionHandler CreateSut() => new(
        _accountRepository,
        _categoryRepository,
        _transactionRepository,
        _unitOfWork);

    private static CreateTransactionCommand ValidCommand(
        Guid accountId,
        Guid categoryId,
        string currency = "USD",
        TransactionType type = TransactionType.Income) => new(
            AccountId:  accountId,
            CategoryId: categoryId,
            Amount:     100m,
            Currency:   currency,
            Type:       type,
            Date:       DateOnly.FromDateTime(DateTime.UtcNow),
            Note:       null);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessWithNewGuid()
    {
        var account  = Account.Create("Checking", "USD");
        var category = Category.Create("Salary", CategoryType.Income);

        _accountRepository
            .GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        _categoryRepository
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        var result = await CreateSut().Handle(ValidCommand(account.Id, category.Id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);

        await _transactionRepository.Received(1)
            .AddAsync(Arg.Any<Transaction>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AccountNotFound_ReturnsNotFoundError()
    {
        var missingId = Guid.NewGuid();

        _accountRepository
            .GetByIdAsync(missingId, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        var result = await CreateSut().Handle(ValidCommand(missingId, Guid.NewGuid()), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldEndWith(".NotFound");

        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CurrencyMismatch_ReturnsValidationError()
    {
        var account  = Account.Create("Checking", "USD");
        var category = Category.Create("Salary", CategoryType.Income);

        _accountRepository
            .GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);
        _categoryRepository
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        // Account is USD; command sends EUR
        var result = await CreateSut().Handle(
            ValidCommand(account.Id, category.Id, currency: "EUR"), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldStartWith("Validation.");

        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_TypeMismatch_ReturnsValidationError()
    {
        var account  = Account.Create("Checking", "USD");
        var category = Category.Create("Groceries", CategoryType.Expense); // Expense category

        _accountRepository
            .GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);
        _categoryRepository
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        // Command type is Income but category type is Expense
        var result = await CreateSut().Handle(
            ValidCommand(account.Id, category.Id, type: TransactionType.Income), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldStartWith("Validation.");

        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
