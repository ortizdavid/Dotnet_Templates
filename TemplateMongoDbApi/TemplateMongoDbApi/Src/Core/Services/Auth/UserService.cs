using MongoDB.Bson;
using TemplateMongoDbApi.Common.Exceptions;
using TemplateMongoDbApi.Common.Helpers;
using TemplateMongoDbApi.Core.Models.Auth;
using TemplateMongoDbApi.Core.Repositories.Auth;

namespace TemplateMongoDbApi.Core.Services.Auth;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly RoleRepository _roleRepository;
    private readonly UserRefreshTokenRepository _refreshTokenRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly FileUploader _imageUploader;
    private readonly IConfiguration _configuration;
    private readonly string _uploadDirectory;

    public UserService(UserRepository repository, UserRefreshTokenRepository refreshTokenRepository,
        RoleRepository roleRepository,
        IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _repository = repository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _contextAccessor = contextAccessor;
        _configuration = configuration;

        _uploadDirectory = _configuration["UploadsDirectory"] + "/Users";
        _imageUploader = new FileUploader(_uploadDirectory, FileExtensions.Images, 5 * CapacityUnit.MegaByte);
    }

    public async Task CreateUser(CreateUserRequest request)
    {
        if (request is null)
        {
            throw new BadRequestException("Create user request cannot be null. Please provide UserName and Password");
        }
        if (!PasswordHelper.IsStrong(request.Password))
        {
            throw new BadRequestException("Password must include: uppercase and lowercase letters, numbers, special characters and  at least 8 characters long.");
        }
        if (await _repository.ExistsRecord("UserName", request.UserName))
        {
            throw new ConflictException($"User '{request.UserName}' already exists.");
        }
        if (await _repository.ExistsRecord("Email", request.Email))
        {
            throw new ConflictException($"Email '{request.Email}' is already in use.");
        }
        var user = new User()
        {
            Role = await _roleRepository.GetByCodeAsync(request!.RoleCode),
            UserName = request.UserName,
            Password = PasswordHelper.Hash(request.Password),
            Email = request.Email,
        };
        await _repository.CreateAsync(user);
    }

    public async Task ChangePassword(ChangePasswordRequest request, string userId)
    {
        if (request is null)
        {
            throw new BadRequestException("Change password request cannot be null. Please provide Password and Confirmation");
        }
        if (!string.Equals(request.NewPassword, request.PasswordConfirmation))
        {
            throw new BadRequestException("Password and Confirmation does not match.");
        }
        if (!PasswordHelper.IsStrong(request.NewPassword))
        {
            throw new BadRequestException("Password must include: uppercase and lowercase letters, numbers, special characters and  at least 8 characters long.");
        }
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }
        user.Password = PasswordHelper.Hash(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task<Pagination<UserResponse>> GetAllUsers(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var users = await _repository.GetAllDataAsync(param.PageSize, param.PageIndex);
        var userResponses = UserMapper.ToResponseList(users);
        var pagination = new Pagination<UserResponse>(userResponses, count, param.PageIndex, param.PageSize, _contextAccessor); 
        return pagination;
    }

    public async Task<UserResponse> GetUserById(string userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' not found");
        }
        var userResponse = UserMapper.ToResponse(user);
        return userResponse;
    }

    public async Task<User> GetUserByName(string userName)
    {
        var user = await _repository.GetDataByNameAsync(userName);
        if (user is null)
        {
            throw new NotFoundException($"User with name '{userName}' not found");
        }
        return user;
    }

    public async Task<User> GetUserByRefreshToken(string token)
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

    public async Task UploadUserImage(IFormFile file, string userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' not found.");
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

    public async Task ActivateUser(string userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' not found");
        }
        if (user.IsActive)
        {
            throw new ConflictException("User is already active");
        }
        user.IsActive = true;
        user.RecoveryToken = Encryption.GenerateRandomToken(150);
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task DeactivateUser(string userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' not found");
        }
        if (!user.IsActive)
        {
            throw new ConflictException("User is already inactive");
        }
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(user);
    }

    public async Task DeleteUser(string userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' not found");
        }
        await _repository.DeleteAsync(user);
    }   

    public async Task<User> GetUserByNameAndPassword(string? userName, string? password)
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

    public async Task CreateUserRefreshToken(User user, string token, DateTime expiryDate)
    {
        var userRefreshToken = new UserRefreshToken()
        {
            UserId = user.UserId,
            Token = token,
            ExpiryDate = expiryDate,
        };
        await _refreshTokenRepository.CreateAsync(userRefreshToken);
        
        user.UserRefreshToken = userRefreshToken;
        await _repository.UpdateAsync(user);
    }   

    public async Task UpdateUserRefreshToken(User user, string newRefreshToken)
    {
        if (string.IsNullOrEmpty(newRefreshToken))
        {
            throw new BadRequestException("New refresh token cannot be null or empty.");
        }
        var userRefreshToken = await _refreshTokenRepository.GetByUserIdAsync(user.UserId);
        if (userRefreshToken is null)
        {
            throw new NotFoundException($"Refresh token for user with ID '{user.UserId}' not found.");
        }
        if (userRefreshToken.IsExpired)
        {
            throw new InvalidOperationException($"Cannot update an expired refresh token for user with ID '{user.UserId}'.");
        }
        if (userRefreshToken.Token == newRefreshToken)
        {
            throw new ConflictException("New refresh token must be different from the current token.");
        }
        userRefreshToken.Token = newRefreshToken;
        userRefreshToken.UpdatedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(userRefreshToken);
    }   

    public async Task ClearUserRefreshToken(string userId)
    {
        var userRefreshToken = await _refreshTokenRepository.GetByUserIdStrAsync(userId);
        if (userRefreshToken is null)
        {
            throw new NotFoundException($"Refresh token for user with ID '{userId}' not found.");
        }
        if (string.IsNullOrEmpty(userRefreshToken.Token))
        {
            return; 
        }
        userRefreshToken.Token = null;
        userRefreshToken.ExpiryDate = null;
        await _refreshTokenRepository.UpdateAsync(userRefreshToken);
    }     
}