using MediatR;

namespace Wallet.Application.Queries.v1.Transactions.GetBalance;

public record GetBalanceQuery(string Email) : IRequest<GetBalanceQueryResponse>
{
    public string Email { get; } = Email;
}