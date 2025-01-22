using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TemplateApi.Core.Models.Auth;

namespace TemplateApi.Core.Services.Auth
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDictionary<string, DateTime> _blacklist = new Dictionary<string, DateTime>();

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(UserData user)
        {
            var userName = user.UserName ?? string.Empty;
            var userId = user.UserId.ToString();
            var role = user.RoleName ?? string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var audience = _jwtSettings.Audience;
            var expiryMinutes = _jwtSettings.ExpiryMinutes;
            var issuer = _jwtSettings.Issuer;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("userId", userId),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = audience,
                Issuer = issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool ValidateAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                // Validate the token
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true
                }, out var validatedToken);

                if (_blacklist.ContainsKey(token)) // Check if the token is blacklisted
                {
                    return false; // Token has been invalidated
                }
                return true; // Token is valid
            }
            catch
            {
                return false; // Token is invalid
            }
        }

        // Invalidate the token by adding it to a blacklist
        public void InvalidateAccessTokenV2(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.");
            }

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                var expiration = jwtToken?.ValidTo ?? DateTime.UtcNow;
                _blacklist[token] = expiration; // Blacklist the token with its expiration date
            }
            catch
            {
                throw new ArgumentException("Invalid token.");
            }
        }

        public void InvalidateAccessToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.");
            }

            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured properly.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var expiration = jwtToken.ValidTo;

                if (expiration > DateTime.UtcNow)
                {
                    _blacklist[token] = expiration;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid token.", ex);
            }
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklist.ContainsKey(token);
        }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; } = 60;
    }
}