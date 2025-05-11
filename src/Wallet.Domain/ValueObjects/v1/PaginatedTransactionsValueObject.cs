namespace Wallet.Domain.ValueObjects.v1;

public record PaginatedTransactionsValueObject(string Email, DateTime? StartDate, DateTime? EndDate, int Page, int PageSize = 10)
{
    public string Email { get; } = Email;
    public DateTime? StartDate { get; } = StartDate;
    public DateTime? EndDate { get; } = EndDate;
    public int Page { get; } = Page;
    public int PageSize { get; } = PageSize;
}