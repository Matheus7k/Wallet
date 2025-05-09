using FluentValidation;

namespace Wallet.Application.Commands.v1.Transactions.v1.AddTransfer;

public class AddTransferCommandValidator : AbstractValidator<AddTransferCommand>
{
    public AddTransferCommandValidator()
    {
        RuleFor(x => x.ToEmail)
            .NotEmpty()
            .WithMessage("a")
            .EmailAddress()
            .WithMessage("b");
        
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("c")
            .GreaterThan(0)
            .WithMessage("d");
    }
}