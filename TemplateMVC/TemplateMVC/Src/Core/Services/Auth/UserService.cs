using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Repositories.Auth;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Services.Auth;

public class UserService : IUserService
{
    private readonly UserRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly FileUploader _imageUploader;
    private readonly IConfiguration _configuration;
    private readonly string _uploadDirectory;

    public UserService(UserRepository repository, IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _repository = repository;
        _contextAccessor = contextAccessor;
        _configuration = configuration;

        _uploadDirectory = _configuration["UploadsDirectory"] + "/Users";
        _imageUploader = new FileUploader(_uploadDirectory, FileExtensions.Images, 5 * CapacityUnit.MEGA_BYTE);
    }

    public async Task CreateUser(CreateUserViewModel viewModel)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Create user viewModel cannot be null. Please provide UserName and Password");
        }
        if (!PasswordHelper.IsStrong(viewModel.Password))
        {
            throw new BadRequestException("Password must include: uppercase and lowercase letters, numbers, special characters and  at least 8 characters long.");
        }
        if (await _repository.ExistsRecord("UserName", viewModel.UserName))
        {
            throw new ConflictException($"User '{viewModel.UserName}' already exists.");
        }
        if (await _repository.ExistsRecord("Email", viewModel.Email))
        {
            throw new ConflictException($"Email '{viewModel.Email}' is already in use.");
        }
        var user = new User()
        {
            RoleId = viewModel.RoleId,
            UserName = viewModel.UserName,
            Password = PasswordHelper.Hash(viewModel.Password),
            Email = viewModel.Email,
        };
        await _repository.CreateAsync(user);
    }

    public async Task UpdateUser(UpdateUserViewModel viewModel, Guid uniqueId)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Update user viewModel cannot be null. Please provide Name, Role and Email");
        }
        var user = await _repository.GetByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found");
        }
        if (await _repository.ExistsRecordExcluded(viewModel.UserName, viewModel.Email, uniqueId))
        {
            throw new ConflictException($"User Name or Email is already in use.");
        }
        user.RoleId = viewModel.RoleId;
        user.UserName = viewModel.UserName;
        user.Email = viewModel.Email;
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task ChangePassword(ChangePasswordViewModel viewModel, Guid uniqueId)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Change password viewModel cannot be null. Please provide Password and Confirmation");
        }
        if (!string.Equals(viewModel.NewPassword, viewModel.PasswordConfirmation))
        {
            throw new BadRequestException("Password and Confirmation does not match.");
        }
        if (!PasswordHelper.IsStrong(viewModel.NewPassword))
        {
            throw new BadRequestException("Password must include: uppercase and lowercase letters, numbers, special characters and  at least 8 characters long.");
        }
        var user = await _repository.GetByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found.");
        }
        user.Password = PasswordHelper.Hash(viewModel.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task<Pagination<UserData>> GetAllUsers(PaginationParam param, SearchFilter filter)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var users = await _repository.GetAllDataSortedAsync(param.PageSize, param.PageIndex, filter.SearchString, filter.SortOrder);
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

    public async Task<User> GetUserByRecoveryToken(string token)
    {
        var user = await _repository.GetByRecoveryTokenAsync(token);
        if (user is null)
        {
            throw new NotFoundException($"User with token '{token}' not found");
        }
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _repository.GetByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException($"User with email '{email}' not found");
        }
        return user;
    }

    public async Task UploadUserImage(IFormFile file, Guid uniqueId)
    {
        var user = await _repository.GetByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found.");
        }
        if (file is null)
        {
            throw new BadRequestException("No file selected.");
        }
        var imageInfo = await _imageUploader.UploadSingleFile(file);
        user.Image = imageInfo.FinalName;
        user.UpdatedAt = DateTime.Now;
        await _repository.UpdateAsync(user);
    }

    public async Task ActivateUser(Guid uniqueId, Guid? loggedUserId)
    {
        if (uniqueId == loggedUserId)
        {
            throw new ConflictException("Cannot activate your own account");
        }
        var user = await _repository.GetByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found");
        }
        if (user.IsActive)
        {
            throw new ConflictException($"User is '{user.UserName}' already active");
        }
        user.IsActive = true;
        user.RecoveryToken = Encryption.GenerateRandomToken(150);
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task DeactivateUser(Guid uniqueId, Guid? loggedUserId)
    {
        if (uniqueId == loggedUserId)
        {
            throw new ConflictException("Cannot activate your own account");
        }
        var user = await _repository.GetByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found");
        }
        if (!user.IsActive)
        {
            throw new ConflictException("User is already inactive");
        }
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task DeleteUser(Guid uniqueId)
    {
        var user = await _repository.GetByUniqueIdAsync(uniqueId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{uniqueId}' not found");
        }
        await _repository.DeleteAsync(user);
    }   
}