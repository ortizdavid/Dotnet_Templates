using System.Net;

namespace TemplateEventDriven.Common.Exceptions;

public class GatewayTimeoutException : AppException
{
    public GatewayTimeoutException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.GatewayTimeout;
    }
}