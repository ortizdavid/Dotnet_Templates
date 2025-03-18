using System.Net;

namespace TemplateMVC.Common.Exceptions;

public class BadGatewayException : AppException
{
    public BadGatewayException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.BadGateway;
    }
}