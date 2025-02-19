using TemplateMVC.Common.Helpers;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Helpers;

namespace TemplateMVC.Core.Services.Auth;

public interface IUserService
{
    public Task CreateUser(CreateUserViewModel viewModel);
    public Task UpdateUser(UpdateUserViewModel viewModel, Guid uniqueId);
    public Task ChangePassword(ChangePasswordViewModel viewModel, Guid uniqueId);
    public Task<Pagination<UserData>> GetAllUsers(PaginationParam param, SearchFilter filter);
    public Task<UserData> GetUserById(int id);
    public Task<UserData> GetUserByUniqueId(Guid uniqueId);
    public Task<UserData> GetUserByName(string userName);
    public Task<User> GetUserByEmail(string email);
    public Task<User> GetUserByRecoveryToken(string token);
    public Task UploadUserImage(IFormFile file, Guid uniqueId);
    public Task ActivateUser(Guid uniqueId, Guid? loggedUserId);
    public Task DeactivateUser(Guid uniqueId, Guid? loggedUserId);
    public Task DeleteUser(Guid uniqueId);
}