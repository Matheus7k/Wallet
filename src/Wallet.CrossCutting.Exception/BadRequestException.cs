using System.Net;

namespace Wallet.CrossCutting.Exception;

public sealed class BadRequestException(string message) : BaseException(message, (int) HttpStatusCode.BadRequest);