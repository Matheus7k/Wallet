using FluentValidation;

namespace Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;

public class PostAuthenticateCommandValidator : AbstractValidator<PostAuthenticateCommand>
{
    public PostAuthenticateCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Campo 'Email' não pode ser vazio.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Campo 'Senha' não pode ser vazio.");
    }
}