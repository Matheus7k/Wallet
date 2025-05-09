using FluentValidation;

namespace Wallet.Application.Commands.v1.Users.AddUser;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name_NotEmpty")
            .Length(2, 255)
            .WithMessage("Name_MinMaxLength");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email_NotEmpty")
            .EmailAddress()
            .WithMessage("Email_Valid")
            .MaximumLength(255)
            .WithMessage("Email_MaxLength");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password_NotEmpty")
            .Length(8, 50)
            .WithMessage("Password_MinMaxLength")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]")
            .WithMessage("Password_Valid");
        
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("BirthDate_NotEmpty")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-12).Date)
            .WithMessage("BirthDate_MinAge");

        RuleFor(x => x.Address.Street)
            .NotEmpty()
            .WithMessage("Street_NotEmpty")
            .MaximumLength(255)
            .WithMessage("Street_MaxLength");
        
        RuleFor(x => x.Address.Number)
            .NotNull()
            .WithMessage("Number_NotEmpty")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Number_MinValue");
        
        RuleFor(x => x.Address.City)
            .NotEmpty()
            .WithMessage("City_NotEmpty")
            .MaximumLength(100)
            .WithMessage("City_MaxLength");
        
        RuleFor(x => x.Address.Country)
            .NotEmpty()
            .WithMessage("Country_NotEmpty")
            .MaximumLength(100)
            .WithMessage("Country_MaxLength");
    }
}