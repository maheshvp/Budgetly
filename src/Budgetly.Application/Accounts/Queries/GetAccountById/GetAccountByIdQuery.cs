using Budgetly.Application.Accounts.Dtos;
using Budgetly.Application.Common.Results;
using MediatR;

namespace Budgetly.Application.Accounts.Queries.GetAccountById;

public sealed record GetAccountByIdQuery(Guid Id) : IRequest<Result<AccountDto>>;
