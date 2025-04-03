using System.Net;

namespace TemplateRedisApi.Common.Exceptions;

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}