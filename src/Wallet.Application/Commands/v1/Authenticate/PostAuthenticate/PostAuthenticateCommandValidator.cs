using FluentValidation;

namespace Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;

public class PostAuthenticateCommandValidator : AbstractValidator<PostAuthenticateCommand>
{
    public PostAuthenticateCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email_NotEmpty");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password_NotEmpty");
    }
}