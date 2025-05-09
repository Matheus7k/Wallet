namespace Wallet.CrossCutting.Exception;

public abstract class BaseException(string message, int statusCode) : System.Exception(message)
{
    public int StatusCode { get; } = statusCode;
}