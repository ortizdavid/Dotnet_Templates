using System.Net;

namespace TemplateRabbitMQApi.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }
}