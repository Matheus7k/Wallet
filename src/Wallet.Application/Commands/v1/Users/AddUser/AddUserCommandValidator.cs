using FluentValidation;

namespace Wallet.Application.Commands.v1.Users.AddUser;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("a")
            .Length(2, 255)
            .WithMessage("b");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("c")
            .EmailAddress()
            .WithMessage("d")
            .MaximumLength(255)
            .WithMessage("e");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("f")
            .Length(8, 50)
            .WithMessage("g")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
            .WithMessage("h");
        
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("i")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-12))
            .WithMessage("j");

        RuleFor(x => x.Address.Street)
            .NotEmpty()
            .WithMessage("k")
            .MaximumLength(255)
            .WithMessage("l");
        
        RuleFor(x => x.Address.Number)
            .NotNull()
            .WithMessage("m")
            .GreaterThanOrEqualTo(0)
            .WithMessage("n");
        
        RuleFor(x => x.Address.City)
            .NotEmpty()
            .WithMessage("o")
            .MaximumLength(100)
            .WithMessage("p");
        
        RuleFor(x => x.Address.Country)
            .NotEmpty()
            .WithMessage("q")
            .MaximumLength(100)
            .WithMessage("r");
    }
}