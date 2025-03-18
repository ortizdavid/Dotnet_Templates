using System.Net;

namespace TemplateEventDriven.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }
}