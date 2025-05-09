using System.Net;

namespace Wallet.CrossCutting.Exception;

public sealed class ConflictException(string message) : BaseException(message, (int) HttpStatusCode.Conflict);