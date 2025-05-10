using FluentValidation;

namespace Wallet.Application.Commands.v1.Transactions.AddTransfer;

public class AddTransferCommandValidator : AbstractValidator<AddTransferCommand>
{
    public AddTransferCommandValidator()
    {
        RuleFor(x => x.ToEmail)
            .NotEmpty()
            .WithMessage("Email_NotEmpty")
            .EmailAddress()
            .WithMessage("Email_Valid");
        
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("Amount_NotEmpty")
            .GreaterThan(0)
            .WithMessage("Amount_MinValue");
    }
}