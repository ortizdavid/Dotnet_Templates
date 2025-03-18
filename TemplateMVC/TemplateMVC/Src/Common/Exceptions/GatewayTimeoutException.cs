using System.Net;

namespace TemplateMVC.Common.Exceptions;

public class GatewayTimeoutException : AppException
{
    public GatewayTimeoutException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.GatewayTimeout;
    }
}