using System.Net;

namespace Wallet.CrossCutting.Exception;

public class NotFoundException(string message) : BaseException(message, (int) HttpStatusCode.NotFound);