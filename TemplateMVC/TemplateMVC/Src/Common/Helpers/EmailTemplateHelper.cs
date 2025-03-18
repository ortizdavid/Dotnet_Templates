namespace TemplateMVC.Common.Helpers;

public class EmailTemplateHelper
{
    public static string RecoveryLink(string? userName, string recoverLink)
    {
        return $"""
            <html>
                <body>
                    <h1>Password Recovery!</h1>
                    <p>Hello, <b>{userName}</b></p>
                    <p>To recover password Click <a href="{recoverLink}">Here</a></p>
                </body>
            </html>
            """;
    }

    public static string ChangePassword(string? userName, string password)
    {
        return $"""
            <html>
                <body>
                    <h1>Password Changed!</h1>
                    <p>Hello, <b>{userName}</b></p>
                    <p>Your new Password is: <b>{password}</b></p>
                </body>
            </html>
            """;
    }
}