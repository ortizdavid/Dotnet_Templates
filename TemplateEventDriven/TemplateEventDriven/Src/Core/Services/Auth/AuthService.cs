using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Common.Messaging.Email;
using TemplateEventDriven.Core.Models.Auth;

namespace TemplateEventDriven.Core.Services.Auth;

public class AuthService
{
    private readonly UserCommandService _userCommandService;
    private readonly UserQueryService _userQueryService;
    private readonly JwtService _jwtService;
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(UserCommandService userCommandService, UserQueryService userQueryService, JwtService jwtService, EmailService emailService, IConfiguration configuration, IHttpContextAccessor contextAccessor)
    {
        _userCommandService = userCommandService;
        _userQueryService = userQueryService;
        _jwtService = jwtService;
        _emailService = emailService;
        _configuration = configuration;
        _contextAccessor = contextAccessor;
    }

    public async Task<TokenResponse> Authenticate(LoginRequest request)
    {
        if (request is null)
        {
            throw new BadRequestException("Login request cannot be null.");
        }
        var user = await _userQueryService.GetUserByNameAndPassword(request.UserName, request.Password);
        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = await GetOrCreateRefreshToken(user);

        // Send success email
        _emailService.SendEmail(user.Email, "Login Success", $"Hello, User '{user.UserName}'! You've logged in successfully.");
        
        return new TokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
    public async Task Logout()
    {
        var userId = GetUserIdFromClaims();
        await _userCommandService.ClearUserRefreshToken(userId);
        var accessToken = _contextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString()?.Replace("Bearer ", "").Trim();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new BadRequestException("Invalid access token. Token is empty or not set.");
        }
        try
        {
            _jwtService.InvalidateAccessTokenV2(accessToken);
        }
        catch (ArgumentException ex)
        {
            throw new BadRequestException($"Token validation failed: {ex.Message}");
        }
        _contextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
        _contextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
    }

    public async Task<TokenResponse> RefreshUserToken(RefreshTokenRequest request)
    {
        if (request is null || string.IsNullOrEmpty(request.Token))
        {
            throw new BadRequestException("Invalid refresh token request.");
        }
        var user = await _userQueryService.GetUserByRefreshToken(request.Token);
        if (user is null || string.IsNullOrEmpty(user.RefreshToken))
        {
            throw new NotFoundException("Invalid or expired refresh token.");
        }
        // Generate a new access token
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = user.RefreshToken;
        return new TokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task RecoverPassword(ChangePasswordRequest request, string token)
    {
        if (request is null)
        {
            throw new BadRequestException("Please provide password and confirmation");
        }
        if (!PasswordHelper.IsStrong(request.NewPassword))
        {
            throw new BadRequestException("Password must be strong");
        }
        if (request.NewPassword != request.PasswordConfirmation)
        {
            throw new BadRequestException("Passwords does not match");
        }
        var user = await _userQueryService.GetUserByRecoveryToken(token);
        await _userCommandService.ChangePassword(request, user.UniqueId);
        _emailService.SendEmail(user.Email, "Password recovery Success", $"Hello, User '{user.UserName}'! You've recovered password in successfully.");
    }

    private async Task<string> GetOrCreateRefreshToken(UserData user)
    {
        if (!string.IsNullOrEmpty(user.RefreshToken))
        {
            return user.RefreshToken;
        }
        var expiryDaysStr = _configuration["JwtSettings:RefreshTokenExpiryDays"];
        if (string.IsNullOrEmpty(expiryDaysStr))
        {
            throw new InvalidOperationException("RefreshTokenExpiryDays is not configured.");
        }
        // create or get
        var expiryDays = int.Parse(expiryDaysStr);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(expiryDays);
        await _userCommandService.CreateUserRefreshToken(user, newRefreshToken, expiryDate);
        return newRefreshToken;
    }

    public async Task<UserData> GetLoggedUser()
    {
        var accessToken = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "").Trim();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new UnauthorizedException("Access token is missing.");
        }

        if (_jwtService.IsTokenBlacklisted(accessToken))
        {
            throw new UnauthorizedException("Access token is invalidated.");
        }

        var userId = GetUserIdFromClaims();
        var user = await _userQueryService.GetUserById(userId);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        return user;
    }

    public bool IsUserAuthenticated()
    {
        return _contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    private int GetUserIdFromClaims()
    {
        var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedException("User is not logged in. Missing user info");
        }
        return userId;
    }
}