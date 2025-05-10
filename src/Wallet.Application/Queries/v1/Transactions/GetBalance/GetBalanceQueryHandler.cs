using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Interfaces.v1.Repositories;

namespace Wallet.Application.Queries.v1.Transactions.GetBalance;

public class GetBalanceQueryHandler(
    IUserQueryRepository userQueryRepository,
    ILogger<GetBalanceQueryHandler> logger) : IRequestHandler<GetBalanceQuery, GetBalanceQueryResponse>
{
    public async Task<GetBalanceQueryResponse> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var wallet = await userQueryRepository.GetUserWalletByEmailAsync(request.Email);
            
            return new GetBalanceQueryResponse(wallet.Balance);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao consulta de saldo.", nameof(GetBalanceQueryResponse));
            throw;
        }
    }
}