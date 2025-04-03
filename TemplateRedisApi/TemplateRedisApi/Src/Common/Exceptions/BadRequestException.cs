using System.Net;

namespace TemplateRedisApi.Common.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException( string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
}