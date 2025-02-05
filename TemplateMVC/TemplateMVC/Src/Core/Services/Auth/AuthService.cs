using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Common.Messaging;
using System.Runtime.Serialization.Formatters;

namespace TemplateMVC.Core.Services.Auth;

public class AuthService
{
    private readonly UserService _userService;
    private readonly EmailService _emailService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(UserService userService, EmailService emailService, IHttpContextAccessor contextAccessor)
    {
        _userService = userService;
        _emailService = emailService;
        _contextAccessor = contextAccessor;
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
        // Send success email
        //_emailService.SendEmail(user?.Email, "Login Success", $"Hello, User '{user?.UserName}'! You've logged in successfully.");
    }
    
    public void Logout()
    {
        var userName = _contextAccessor?.HttpContext?.Session.GetString("UserName");
        if (string.IsNullOrEmpty(userName))
        {
            throw new NotFoundException("UserName is not set");
        }
        _contextAccessor?.HttpContext?.Session.Remove("UserName");
        _contextAccessor?.HttpContext?.Session.Clear();
    }

    public async Task RecoverPassword(ChangePasswordViewModel viewModel, string token)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Please provide password and confirmation");
        }
        if (!PasswordHelper.IsStrong(viewModel.NewPassword))
        {
            throw new BadRequestException("Password must be strong");
        }
        if (viewModel.NewPassword != viewModel.PasswordConfirmation)
        {
            throw new BadRequestException("Passwords does not match");
        }
        var user = await _userService.GetUserByRecoveryToken(token);
        await _userService.ChangePassword(viewModel, user.UniqueId);
        _emailService.SendEmail(user.Email, "Password recovery Success", $"Hello, User '{user.UserName}'! You've recovered password in successfully.");
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

}