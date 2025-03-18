using System.Net;

namespace TemplateEventDriven.Common.Exceptions;

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}