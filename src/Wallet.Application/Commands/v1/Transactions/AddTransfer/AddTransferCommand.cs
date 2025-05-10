using System.Text.Json.Serialization;
using MediatR;

namespace Wallet.Application.Commands.v1.Transactions.AddTransfer;

public record AddTransferCommand : IRequest
{
    [JsonIgnore]
    public string FromEmail { get; private set; } = null!;
    
    public string ToEmail { get; init; } = null!;
    public decimal Amount { get; init; }

    public AddTransferCommand SetFromEmail(string email)
    {
        FromEmail = email;
        return this;
    }
}