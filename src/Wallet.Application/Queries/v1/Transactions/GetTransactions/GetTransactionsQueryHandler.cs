using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Interfaces.v1.Repositories;

namespace Wallet.Application.Queries.v1.Transactions.GetTransactions;

public class GetTransactionsQueryHandler(
    IUserQueryRepository userQueryRepository,
    ILogger<GetTransactionsQueryHandler> logger) : IRequestHandler<GetTransactionsQuery, GetTransactionsQueryResponse>
{
    public async Task<GetTransactionsQueryResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var totalRows = await userQueryRepository.GetTotalRowsAsync(request.Email);
            
            var (sentTransactions, receivedTransactions) = 
                await userQueryRepository.GetPaginatedTransactionsAsync(new(request.Email, request.StartDate, request.EndDate, request.Page, request.PageSize));

            return new GetTransactionsQueryResponse(totalRows, request.Page, sentTransactions, receivedTransactions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao buscar histórico de transações. Request: {Request}", nameof(GetTransactionsQueryHandler), request);
            throw;
        }
    }
}