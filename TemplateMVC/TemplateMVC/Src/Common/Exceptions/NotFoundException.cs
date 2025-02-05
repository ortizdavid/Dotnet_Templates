using System.Net;

namespace TemplateMVC.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }
}