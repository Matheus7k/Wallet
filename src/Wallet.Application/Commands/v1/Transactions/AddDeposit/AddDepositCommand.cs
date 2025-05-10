using System.Text.Json.Serialization;
using MediatR;

namespace Wallet.Application.Commands.v1.Transactions.AddDeposit;

public class AddDepositCommand : IRequest
{
    [JsonIgnore]
    public string Email { get; set; } = null!;
    
    public decimal Amount { get; set; }

    public AddDepositCommand SetEmail(string email)
    {
        Email = email;
        return this;
    }
}