using Budgetly.Domain.ValueObjects;

namespace Budgetly.Domain.Tests.ValueObjects;

public sealed class MoneyTests
{
    [Fact]
    public void Constructor_ValidCurrency_UppercasesIt()
    {
        var money = new Money(10m, "usd");
        money.Currency.ShouldBe("USD");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("US")]
    [InlineData("USDD")]
    [InlineData("US1")]
    [InlineData("U$D")]
    public void Constructor_InvalidCurrency_ThrowsArgumentException(string badCurrency)
    {
        Should.Throw<ArgumentException>(() => new Money(10m, badCurrency));
    }

    [Fact]
    public void Constructor_NullCurrency_ThrowsArgumentException()
    {
        Should.Throw<ArgumentException>(() => new Money(10m, null!));
    }

    [Fact]
    public void Add_SameCurrency_ReturnsCorrectSum()
    {
        var a = new Money(100m, "USD");
        var b = new Money(50m, "USD");

        var result = a.Add(b);

        result.Amount.ShouldBe(150m);
        result.Currency.ShouldBe("USD");
    }

    [Fact]
    public void Add_DifferentCurrencies_ThrowsInvalidOperationException()
    {
        var usd = new Money(100m, "USD");
        var eur = new Money(50m, "EUR");

        Should.Throw<InvalidOperationException>(() => usd.Add(eur));
    }

    [Fact]
    public void Subtract_SameCurrency_ReturnsCorrectDifference()
    {
        var a = new Money(100m, "USD");
        var b = new Money(30m, "USD");

        var result = a.Subtract(b);

        result.Amount.ShouldBe(70m);
        result.Currency.ShouldBe("USD");
    }

    [Fact]
    public void Subtract_DifferentCurrencies_ThrowsInvalidOperationException()
    {
        var usd = new Money(100m, "USD");
        var eur = new Money(30m, "EUR");

        Should.Throw<InvalidOperationException>(() => usd.Subtract(eur));
    }

    [Fact]
    public void TwoMoneyInstances_SameValues_AreEqual()
    {
        var a = new Money(100m, "USD");
        var b = new Money(100m, "USD");

        a.ShouldBe(b);
    }

    [Fact]
    public void TwoMoneyInstances_DifferentValues_AreNotEqual()
    {
        var a = new Money(100m, "USD");
        var b = new Money(200m, "USD");

        a.ShouldNotBe(b);
    }
}
