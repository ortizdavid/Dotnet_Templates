using System.Net;

namespace TemplateEventDriven.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}