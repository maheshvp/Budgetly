using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using Budgetly.Application.Transactions.Dtos;
using MediatR;

namespace Budgetly.Application.Transactions.Queries.GetTransactions;

public sealed class GetTransactionsHandler(ITransactionRepository transactionRepository)
    : IRequestHandler<GetTransactionsQuery, Result<IReadOnlyList<TransactionDto>>>
{
    public async Task<Result<IReadOnlyList<TransactionDto>>> Handle(
        GetTransactionsQuery query,
        CancellationToken cancellationToken)
    {
        var transactions = await transactionRepository.GetAllAsync(
            query.AccountId, query.From, query.To, cancellationToken);

        var dtos = transactions.Select(TransactionDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<TransactionDto>>(dtos);
    }
}
