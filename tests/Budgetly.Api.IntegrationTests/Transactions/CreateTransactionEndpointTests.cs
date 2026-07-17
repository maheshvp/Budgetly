using System.Net;
using System.Net.Http.Json;
using Budgetly.Application.Accounts.Dtos;
using Budgetly.Domain.Enums;
using Budgetly.Infrastructure.Persistence.Configurations;

namespace Budgetly.Api.IntegrationTests.Transactions;

public sealed class CreateTransactionEndpointTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateTransaction_ThenGet_UpdatesAccountBalance()
    {
        var createAccountResponse = await _client.PostAsJsonAsync(
            "/api/accounts",
            new { Name = "Checking", Currency = "USD" });
        createAccountResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var accountId = (await createAccountResponse.Content.ReadFromJsonAsync<CreatedIdResponse>())!.Id;

        var createTransactionResponse = await _client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                AccountId = accountId,
                CategoryId = CategoryConfiguration.SalaryId,
                Amount = 100m,
                Currency = "USD",
                Type = TransactionType.Income,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = "Paycheck"
            });
        createTransactionResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var transactionId = (await createTransactionResponse.Content.ReadFromJsonAsync<CreatedIdResponse>())!.Id;

        var getTransactionResponse = await _client.GetAsync($"/api/transactions/{transactionId}");
        getTransactionResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var getAccountResponse = await _client.GetAsync($"/api/accounts/{accountId}");
        getAccountResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var account = await getAccountResponse.Content.ReadFromJsonAsync<AccountDto>();

        account!.Balance.ShouldBe(100m);
    }

    [Fact]
    public async Task CreateTransaction_MissingAccount_ReturnsNotFound()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                AccountId = Guid.NewGuid(),
                CategoryId = CategoryConfiguration.SalaryId,
                Amount = 50m,
                Currency = "USD",
                Type = TransactionType.Income,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = (string?)null
            });

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTransaction_CurrencyMismatch_ReturnsBadRequest()
    {
        var createAccountResponse = await _client.PostAsJsonAsync(
            "/api/accounts",
            new { Name = "Euro Account", Currency = "EUR" });
        var accountId = (await createAccountResponse.Content.ReadFromJsonAsync<CreatedIdResponse>())!.Id;

        var response = await _client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                AccountId = accountId,
                CategoryId = CategoryConfiguration.SalaryId,
                Amount = 50m,
                Currency = "USD",
                Type = TransactionType.Income,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = (string?)null
            });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    private sealed record CreatedIdResponse(Guid Id);
}
