using System.Net;

namespace TemplateNatsApi.Common.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message) 
    {
        StatusCode = (int)HttpStatusCode.Conflict;
    }
}