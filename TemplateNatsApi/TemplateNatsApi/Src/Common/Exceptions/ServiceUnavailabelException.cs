using System.Net;

namespace TemplateNatsApi.Common.Exceptions;

public class ServiceUnavailabelException : AppException
{
    public ServiceUnavailabelException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.ServiceUnavailable;
    }
}