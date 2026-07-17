using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Budgetly.Domain.ValueObjects;

namespace Budgetly.Domain.Tests.Entities;

public sealed class TransactionTests
{
    private static readonly Guid AccountId = Guid.NewGuid();
    private static readonly Guid CategoryId = Guid.NewGuid();
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly Money ValidAmount = new(100m, "USD");

    [Fact]
    public void Create_ValidInputs_ReturnsTransactionWithCorrectProperties()
    {
        var transaction = Transaction.Create(
            AccountId, CategoryId, ValidAmount, TransactionType.Income, Today, "bonus");

        transaction.Id.ShouldNotBe(Guid.Empty);
        transaction.AccountId.ShouldBe(AccountId);
        transaction.CategoryId.ShouldBe(CategoryId);
        transaction.Amount.ShouldBe(ValidAmount);
        transaction.Type.ShouldBe(TransactionType.Income);
        transaction.Date.ShouldBe(Today);
        transaction.Note.ShouldBe("bonus");
    }

    [Fact]
    public void Create_NullNote_IsAllowed()
    {
        var transaction = Transaction.Create(
            AccountId, CategoryId, ValidAmount, TransactionType.Expense, Today, note: null);

        transaction.Note.ShouldBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Create_AmountLessThanOrEqualToZero_ThrowsArgumentException(decimal badAmount)
    {
        var badMoney = new Money(badAmount, "USD");

        Should.Throw<ArgumentException>(() =>
            Transaction.Create(AccountId, CategoryId, badMoney, TransactionType.Income, Today, null));
    }

    [Fact]
    public void Create_SmallPositiveAmount_Succeeds()
    {
        var smallAmount = new Money(0.01m, "USD");

        var transaction = Transaction.Create(
            AccountId, CategoryId, smallAmount, TransactionType.Expense, Today, null);

        transaction.Amount.Amount.ShouldBe(0.01m);
    }

    [Fact]
    public void Create_NoteExceeds250Chars_ThrowsArgumentException()
    {
        var longNote = new string('X', 251);

        Should.Throw<ArgumentException>(() =>
            Transaction.Create(AccountId, CategoryId, ValidAmount, TransactionType.Income, Today, longNote));
    }

    [Fact]
    public void Create_NoteExactly250Chars_Succeeds()
    {
        var maxNote = new string('X', 250);

        var transaction = Transaction.Create(
            AccountId, CategoryId, ValidAmount, TransactionType.Income, Today, maxNote);

        transaction.Note!.Length.ShouldBe(250);
    }

    [Fact]
    public void Create_TwoCalls_ProduceDifferentIds()
    {
        var t1 = Transaction.Create(AccountId, CategoryId, ValidAmount, TransactionType.Income, Today, null);
        var t2 = Transaction.Create(AccountId, CategoryId, ValidAmount, TransactionType.Income, Today, null);

        t1.Id.ShouldNotBe(t2.Id);
    }
}
