using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Budgetly.Domain.ValueObjects;

namespace Budgetly.Domain.Tests.Entities;

public sealed class AccountTests
{
    [Fact]
    public void Create_ValidInputs_ReturnsAccountWithZeroBalance()
    {
        var account = Account.Create("Checking", "USD");

        account.Id.ShouldNotBe(Guid.Empty);
        account.Name.ShouldBe("Checking");
        account.Balance.Amount.ShouldBe(0m);
        account.Balance.Currency.ShouldBe("USD");
        account.CreatedAtUtc.Kind.ShouldBe(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyOrWhitespaceName_ThrowsArgumentException(string badName)
    {
        Should.Throw<ArgumentException>(() => Account.Create(badName, "USD"));
    }

    [Fact]
    public void Create_NameExceeds100Chars_ThrowsArgumentException()
    {
        var longName = new string('X', 101);
        Should.Throw<ArgumentException>(() => Account.Create(longName, "USD"));
    }

    [Fact]
    public void Create_InvalidCurrency_ThrowsArgumentException()
    {
        Should.Throw<ArgumentException>(() => Account.Create("Savings", "INVALID"));
    }

    [Fact]
    public void Apply_IncomeTransaction_IncreasesBalance()
    {
        var account = Account.Create("Checking", "USD");
        var transaction = Transaction.Create(
            account.Id,
            Guid.NewGuid(),
            new Money(500m, "USD"),
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            note: null);

        account.Apply(transaction);

        account.Balance.Amount.ShouldBe(500m);
    }

    [Fact]
    public void Apply_ExpenseTransaction_DecreasesBalance()
    {
        var account = Account.Create("Checking", "USD");
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var categoryId = Guid.NewGuid();

        account.Apply(Transaction.Create(account.Id, categoryId, new Money(1000m, "USD"), TransactionType.Income, date, null));
        account.Apply(Transaction.Create(account.Id, categoryId, new Money(300m, "USD"), TransactionType.Expense, date, null));

        account.Balance.Amount.ShouldBe(700m);
    }

    [Fact]
    public void Apply_MultipleIncomeTransactions_AccumulatesBalance()
    {
        var account = Account.Create("Checking", "USD");
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var categoryId = Guid.NewGuid();

        account.Apply(Transaction.Create(account.Id, categoryId, new Money(100m, "USD"), TransactionType.Income, date, null));
        account.Apply(Transaction.Create(account.Id, categoryId, new Money(200m, "USD"), TransactionType.Income, date, null));
        account.Apply(Transaction.Create(account.Id, categoryId, new Money(50m, "USD"), TransactionType.Income, date, null));

        account.Balance.Amount.ShouldBe(350m);
    }

    [Fact]
    public void Apply_CurrencyMismatch_ThrowsInvalidOperationException()
    {
        var account = Account.Create("Checking", "USD");
        var transaction = Transaction.Create(
            account.Id,
            Guid.NewGuid(),
            new Money(100m, "EUR"),
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            note: null);

        Should.Throw<InvalidOperationException>(() => account.Apply(transaction));
    }

    [Fact]
    public void Apply_CurrencyMismatch_DoesNotMutateBalance()
    {
        var account = Account.Create("Checking", "USD");
        var transaction = Transaction.Create(
            account.Id,
            Guid.NewGuid(),
            new Money(100m, "EUR"),
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            note: null);

        try { account.Apply(transaction); } catch (InvalidOperationException) { }

        account.Balance.Amount.ShouldBe(0m);
    }
}
