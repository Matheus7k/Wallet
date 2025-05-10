using FluentValidation;

namespace Wallet.Application.Queries.v1.Transactions.GetTransactions;

public class GetTransactionsQueryValidator : AbstractValidator<GetTransactionsQuery>
{
    public GetTransactionsQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("StartDate_NotNull")
            .When(x => x.EndDate.HasValue);
            
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("StartDate_MaxValue");
        
        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage("EndDate_NotNull")
            .When(x => x.StartDate.HasValue);
            
        RuleFor(x => x.EndDate)
            .LessThanOrEqualTo(x => DateTime.UtcNow.AddDays(1).AddTicks(-1))
            .WithMessage("EndDate_MaxValue");
    }
}