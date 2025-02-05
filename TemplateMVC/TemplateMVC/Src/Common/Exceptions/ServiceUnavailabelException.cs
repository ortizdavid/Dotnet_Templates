using System.Net;

namespace TemplateMVC.Common.Exceptions;

public class ServiceUnavailabelException : AppException
{
    public ServiceUnavailabelException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.ServiceUnavailable;
    }
}