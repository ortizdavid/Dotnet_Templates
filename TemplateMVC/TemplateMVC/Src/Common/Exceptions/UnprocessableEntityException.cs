using System.Net;

namespace TemplateMVC.Common.Exceptions;

public class UnprocessableEntityException : AppException
{
    public UnprocessableEntityException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.UnprocessableEntity;
    }
}