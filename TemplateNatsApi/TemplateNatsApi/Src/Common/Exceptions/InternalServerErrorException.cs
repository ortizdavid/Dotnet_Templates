using System.Net;

namespace TemplateNatsApi.Common.Exceptions;

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}