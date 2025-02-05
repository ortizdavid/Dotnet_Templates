using System.Net;

namespace TemplateMVC.Common.Exceptions;

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}