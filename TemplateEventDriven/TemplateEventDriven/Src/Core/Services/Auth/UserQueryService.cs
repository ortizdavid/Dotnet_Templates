using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Auth;
using TemplateEventDriven.Core.Repositories.Auth;

namespace TemplateEventDriven.Core.Services.Auth;

public class UserQueryService
{
    private readonly UserRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserQueryService(UserRepository repository, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _contextAccessor = contextAccessor;
    }

    public async Task<Pagination<UserData>> GetAllUsers(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var users = await _repository.GetAllDataAsync(param.PageSize, param.PageIndex);
        var pagination = new Pagination<UserData>(users, count, param.PageIndex, param.PageSize, _contextAccessor); 
        return pagination;
    }

    public async Task<UserData> GetUserById(int id)
    {
        var user = await _repository.GetDataByIdAsync(id);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{id}' not found");
        }
        return user;
    }

    public async Task<UserData> GetUserByUniqueId(Guid uniqueId)
    {
        var user = await _repository.GetDataByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found");
        }
        return user;
    }

    public async Task<UserData> GetUserByName(string userName)
    {
        var user = await _repository.GetDataByNameAsync(userName);
        if (user is null)
        {
            throw new NotFoundException($"User with name '{userName}' not found");
        }
        return user;
    }

    public async Task<UserData> GetUserByRefreshToken(string token)
    {
        var user = await _repository.GetDataByRefreshTokenAsync(token);
        if (user is null)
        {
            throw new NotFoundException($"User refresh token not found");
        }
        return user;
    }

    public async Task<User> GetUserByRecoveryToken(string token)
    {
        var user = await _repository.GetByRecoveryTokenAsync(token);
        if (user is null)
        {
            throw new NotFoundException($"User with token '{token}' not found");
        }
        return user;
    }

    public async Task<UserData> GetUserByNameAndPassword(string? userName, string? password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            throw new BadRequestException("Username and Password are required.");
        }
        var user = await _repository.GetDataByNameAsync(userName);
        if (user is null || !PasswordHelper.Verify(password, user.Password))
        {
            throw new UnauthorizedException("Invalid username or password.");
        }
        return user;
    }
}