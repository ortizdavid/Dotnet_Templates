using System.Net;

namespace TemplateApi.Common.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.Forbidden;
    }
}