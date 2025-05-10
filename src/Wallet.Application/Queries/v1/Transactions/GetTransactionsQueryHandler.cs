using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Interfaces.v1.Repositories;

namespace Wallet.Application.Queries.v1.Transactions;

public class GetTransactionsQueryHandler(
    IUserQueryRepository userQueryRepository,
    ILogger<GetTransactionsQueryHandler> logger) : IRequestHandler<GetTransactionsQuery, GetTransactionsQueryResponse>
{
    public async Task<GetTransactionsQueryResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var transactions = await userQueryRepository.GetTransactionsAsync(request.Email, request.StartDate, request.EndDate);

            return new GetTransactionsQueryResponse(transactions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao buscar histórico de transações. Request: {Request}", nameof(GetTransactionsQueryHandler), request);
            throw;
        }
    }
}