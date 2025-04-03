using System.Net;

namespace TemplateRedisApi.Common.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.Forbidden;
    }
}