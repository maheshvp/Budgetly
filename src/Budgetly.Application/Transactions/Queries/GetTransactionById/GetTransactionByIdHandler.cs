using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using Budgetly.Application.Transactions.Dtos;
using Budgetly.Domain.Entities;
using MediatR;

namespace Budgetly.Application.Transactions.Queries.GetTransactionById;

public sealed class GetTransactionByIdHandler(ITransactionRepository transactionRepository)
    : IRequestHandler<GetTransactionByIdQuery, Result<TransactionDto>>
{
    public async Task<Result<TransactionDto>> Handle(
        GetTransactionByIdQuery query,
        CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(query.Id, cancellationToken);
        if (transaction is null)
            return Result.Failure<TransactionDto>(Error.NotFound(nameof(Transaction), query.Id));

        return Result.Success(TransactionDto.FromEntity(transaction));
    }
}
