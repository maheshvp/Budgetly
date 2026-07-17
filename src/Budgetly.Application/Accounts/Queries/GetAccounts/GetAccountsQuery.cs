using Budgetly.Application.Accounts.Dtos;
using Budgetly.Application.Common.Results;
using MediatR;

namespace Budgetly.Application.Accounts.Queries.GetAccounts;

public sealed record GetAccountsQuery : IRequest<Result<IReadOnlyList<AccountDto>>>;
