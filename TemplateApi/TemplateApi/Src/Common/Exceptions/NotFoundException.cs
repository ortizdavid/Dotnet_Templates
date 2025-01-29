using System.Net;

namespace TemplateApi.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }
}