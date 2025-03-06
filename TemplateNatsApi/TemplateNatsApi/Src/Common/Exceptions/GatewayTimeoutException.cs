using System.Net;

namespace TemplateNatsApi.Common.Exceptions;

public class GatewayTimeoutException : AppException
{
    public GatewayTimeoutException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.GatewayTimeout;
    }
}