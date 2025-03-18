namespace TemplateSimpleMVC.Helpers;

public class PasswordHelper 
{
    public static string Hash(string? password)
    {
        if(string.IsNullOrEmpty(password)) {
            throw new ArgumentNullException("Password must have a value");
        }
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string? password, string? hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }   

    public static bool IsStrong(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) 
        {
            return false;
        }
        var hasMinimumLength = password.Length >= 8;
        var hasUpperCase = password.Any(char.IsUpper);
        var hasLowerCase = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        var hasSpecialChar = password.Any(ch=> !char.IsLetterOrDigit(ch));
        return hasMinimumLength && hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
    }
}

