using System.Net;

namespace TemplateRedisApi.Common.Exceptions;

public class ServiceUnavailabelException : AppException
{
    public ServiceUnavailabelException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.ServiceUnavailable;
    }
}