using FluentValidation;

namespace Wallet.Application.Commands.v1.Transactions.AddDeposit;

public class AddDepositCommandValidator : AbstractValidator<AddDepositCommand>
{
    public AddDepositCommandValidator()
    {
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("Amount_NotEmpty")
            .GreaterThan(0)
            .WithMessage("Amount_MinValue");
    }
}