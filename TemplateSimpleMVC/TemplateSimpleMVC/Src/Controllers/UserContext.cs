using TemplateSimpleMVC.Models;
using TemplateSimpleMVC.Repositories;

namespace TemplateSimpleMVC.Controllers;

public class UserContext
{
    private readonly UserRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserContext(UserRepository repository, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _contextAccessor = contextAccessor;
    }

    public User? GetLoggedUser()
    {
        var userName = _contextAccessor.HttpContext?.Session.GetString("UserName");
        var user = _repository.FindByName(userName);
        return user;
    }
}