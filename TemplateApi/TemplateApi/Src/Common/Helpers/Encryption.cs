using System;
using System.Security.Cryptography;

namespace TemplateApi.Helpers;

public class Encryption
{
    public static Guid GenerateUUID()
    {
        return Guid.NewGuid();
    }

    public static string GenerateRandomToken(int length)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var tokenData = new byte[length];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }
    }
}

