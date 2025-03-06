using System.Net;

namespace TemplateNatsApi.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}