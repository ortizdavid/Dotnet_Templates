using System.Net;

namespace TemplateMongoDbApi.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}