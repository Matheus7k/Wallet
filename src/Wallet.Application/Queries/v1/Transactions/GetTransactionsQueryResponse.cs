using Wallet.Domain.Entities.v1;

namespace Wallet.Application.Queries.v1.Transactions;

public record GetTransactionsQueryResponse(IEnumerable<WalletTransaction> SentTransactions, IEnumerable<WalletTransaction> ReceivedTransactions);