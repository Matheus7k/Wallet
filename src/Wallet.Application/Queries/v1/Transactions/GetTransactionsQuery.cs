using System.Text.Json.Serialization;
using MediatR;

namespace Wallet.Application.Queries.v1.Transactions;

public record GetTransactionsQuery : IRequest<GetTransactionsQueryResponse>
{
    [JsonIgnore] 
    public string Email { get; private set; } = null!;

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public GetTransactionsQuery SetEmail(string email)
    {
        Email = email;
        return this;
    }
}