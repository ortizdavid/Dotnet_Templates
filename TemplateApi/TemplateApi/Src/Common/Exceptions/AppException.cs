namespace TemplateApi.Common.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; set; }
    
    public AppException(string message) : base(message) {}
}