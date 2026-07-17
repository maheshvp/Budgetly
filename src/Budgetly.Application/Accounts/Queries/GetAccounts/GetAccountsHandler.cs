using Budgetly.Application.Accounts.Dtos;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using MediatR;

namespace Budgetly.Application.Accounts.Queries.GetAccounts;

public sealed class GetAccountsHandler(IAccountRepository accountRepository)
    : IRequestHandler<GetAccountsQuery, Result<IReadOnlyList<AccountDto>>>
{
    public async Task<Result<IReadOnlyList<AccountDto>>> Handle(
        GetAccountsQuery query,
        CancellationToken cancellationToken)
    {
        var accounts = await accountRepository.GetAllAsync(cancellationToken);
        var dtos = accounts.Select(AccountDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<AccountDto>>(dtos);
    }
}
