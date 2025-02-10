using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Common.Messaging;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Services.Auth;

public class AuthService
{
    private readonly UserService _userService;
    private readonly EmailService _emailService;
    private readonly UrlHelperService _urlService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(UserService userService, EmailService emailService, UrlHelperService urlService, IHttpContextAccessor contextAcessor)
    {
        _userService = userService;
        _emailService = emailService;
        _urlService = urlService;
        _contextAccessor = contextAcessor;
    }

    public async Task Authenticate(LoginViewModel viewModel)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Login viewModel cannot be null.");
        }
        var userName = viewModel.UserName ?? string.Empty;
        var user = await _userService.GetUserByName(userName);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }
        if (user is null || !PasswordHelper.Verify(viewModel.Password, user.Password))
        {
            throw new UnauthorizedException("Invalid username or password.");
        }
        _contextAccessor?.HttpContext?.Session.SetString("UserName", user.UserName??"");
        _contextAccessor?.HttpContext?.Session.SetString("RoleName", user.RoleName??"");
    }
    
    public void Logout()
    {
        var userName = _contextAccessor?.HttpContext?.Session.GetString("UserName");
        if (string.IsNullOrEmpty(userName))
        {
            throw new NotFoundException("UserName session key is not set");
        }
        _contextAccessor?.HttpContext?.Session.Remove("UserName");
        _contextAccessor?.HttpContext?.Session.Clear();
    }

    public async Task GetRecoverLink(GetRecoverLinkViewModel viewModel)
    {
        if (string.IsNullOrEmpty(viewModel.Email))
        {
            throw new BadRequestException("Please provide an Email");
        }
        var user = await _userService.GetUserByEmail(viewModel.Email);
       
        if (user is null)
        {
            throw new NotFoundException($"User with email '{viewModel.Email}' not found");
        }
        var recoverLink = _urlService.GetPasswordRecoveryLink(user.RecoveryToken);
        _emailService.SendEmail(user?.Email, "Recover Link", RecoveryLinkTemplate(user?.UserName, recoverLink));
    }

    public async Task RecoverPassword(ChangePasswordViewModel viewModel, string token)
    {
        if (viewModel.NewPassword is null || viewModel.PasswordConfirmation is null)
        {
            throw new BadRequestException("Please provide password and confirmation");
        }
        if (!PasswordHelper.IsStrong(viewModel.NewPassword))
        {
            throw new BadRequestException("Password must be strong");
        }
        if (viewModel.NewPassword != viewModel.PasswordConfirmation)
        {
            throw new BadRequestException("Password and confirmation does not match");
        }
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new BadRequestException("Token is missing");
        }
        var user = await _userService.GetUserByRecoveryToken(token);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        await _userService.ChangePassword(viewModel, user.UniqueId);
        _emailService.SendEmail(user.Email, "Password Recovery Success", NewPasswordTemplate(user.UserName, viewModel.NewPassword));
    }

    public async Task<UserData?> GetLoggedUser()
    {
        var userName = _contextAccessor.HttpContext?.Session.GetString("UserName") ?? string.Empty;
        var user = await _userService.GetUserByName(userName);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }
        return user;
    }

    private string RecoveryLinkTemplate(string? userName, string recoverLink)
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

    private string NewPasswordTemplate(string? userName, string password)
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