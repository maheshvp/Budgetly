using Budgetly.Application.Accounts.Dtos;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using Budgetly.Domain.Entities;
using MediatR;

namespace Budgetly.Application.Accounts.Queries.GetAccountById;

public sealed class GetAccountByIdHandler(IAccountRepository accountRepository)
    : IRequestHandler<GetAccountByIdQuery, Result<AccountDto>>
{
    public async Task<Result<AccountDto>> Handle(
        GetAccountByIdQuery query,
        CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(query.Id, cancellationToken);
        if (account is null)
            return Result.Failure<AccountDto>(Error.NotFound(nameof(Account), query.Id));

        return Result.Success(AccountDto.FromEntity(account));
    }
}
