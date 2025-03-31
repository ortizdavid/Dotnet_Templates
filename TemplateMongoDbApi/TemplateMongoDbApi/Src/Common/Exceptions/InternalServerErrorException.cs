using System.Net;

namespace TemplateMongoDbApi.Common.Exceptions;

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}