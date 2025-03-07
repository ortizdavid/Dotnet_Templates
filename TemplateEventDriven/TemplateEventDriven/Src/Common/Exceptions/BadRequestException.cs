using System.Net;

namespace TemplateEventDriven.Common.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException( string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
}