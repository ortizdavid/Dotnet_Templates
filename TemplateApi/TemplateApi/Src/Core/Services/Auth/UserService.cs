using TemplateApi.Common.Exceptions;
using TemplateApi.Helpers;
using TemplateApi.Core.Models.Auth;
using TemplateApi.Core.Repositories.Auth;

namespace TemplateApi.Core.Services.Auth
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly UserRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly FileUploader _imageUploader;
        private readonly IConfiguration _configuration;
        private readonly string _uploadDirectory;

        public UserService(UserRepository repository, UserRefreshTokenRepository refreshTokenRepository,
            IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _contextAccessor = contextAccessor;
            _configuration = configuration;

            _uploadDirectory = _configuration["UploadsDirectory"] + "/Users";
            _imageUploader = new FileUploader(_uploadDirectory, FileExtensions.Images, 5 * CapacityUnit.MEGA_BYTE);
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
                RoleId = request.RoleId,
                UserName = request.UserName,
                Password = PasswordHelper.Hash(request.Password),
                Email = request.Email,
            };
            await _repository.CreateAsync(user);
        }

        public async Task ChangePassword(ChangePasswordRequest request, Guid uniqueId)
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
            var user = await _repository.GetByUniqueIdAsync(uniqueId);
            if (user is null)
            {
                throw new NotFoundException($"User with ID '{uniqueId}' not found.");
            }
            user.Password = PasswordHelper.Hash(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);
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

        public async Task ActivateUser(Guid uniqueId)
        {
            var user = await _repository.GetByUniqueIdAsync(uniqueId);
            if (user is null)
            {
                throw new NotFoundException($"User with ID '{uniqueId}' not found");
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

        public async Task DeactivateUser(Guid uniqueId)
        {
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

        public async Task CreateUserRefreshToken(UserData user, string token, DateTime expiryDate)
        {
            var userRefreshToken = new UserRefreshToken()
            {
                UserId = user.UserId,
                Token = token,
                ExpiryDate = expiryDate,
            };
            await _refreshTokenRepository.CreateAsync(userRefreshToken);
        }   

        public async Task UpdateUserRefreshToken(UserData user, string newRefreshToken)
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

        public async Task ClearUserRefreshToken(int userId)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByUserIdAsync(userId);
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
}