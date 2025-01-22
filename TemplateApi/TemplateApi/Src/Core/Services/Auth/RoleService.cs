using TemplateApi.Common.Exceptions;
using TemplateApi.Helpers;
using TemplateApi.Core.Models.Auth;
using TemplateApi.Core.Repositories.Auth;

namespace TemplateApi.Core.Services.Auth
{
    public class RoleService
    {
        private readonly RoleRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleService(RoleRepository repository, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
        }

        public async Task CreateRole(RoleRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Please provide role name and code");
            }
            if (await _repository.ExistsRecord("RoleName", request.RoleName))
            {
                throw new ConflictException($"Role '{request.RoleName}' already exists");
            }
            if (await _repository.ExistsRecord("Code", request.Code))
            {
                throw new ConflictException($"Code '{request.Code}' already exists");
            }
            var role = new Role()
            {
                RoleName = request.RoleName,
                Code = request.Code,
            };
            await _repository.CreateAsync(role);
        }

        public async Task UpdateRole(RoleRequest request, Guid uniqueId)
        {
            if (request is null)
            {
                throw new BadRequestException("Please provide role name and code");
            }
            var role = await _repository.GetByUniqueIdAsync(uniqueId);
            if (role is null)
            {
                throw new NotFoundException($"Role with ID '{uniqueId}' not found");
            }
            role.RoleName = request.RoleName;
            role.Code = request.Code;
            await _repository.UpdateAsync(role); 
        }

        public async Task<Pagination<Role>> GetAllRoles(PaginationParam param)
        {
            if (param is null)
            {
                throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
            }
            var count = await _repository.CountAsync();
            var roles = await _repository.GetAllDataAsync(param.PageSize, param.PageIndex);
            var pagination = new Pagination<Role>(roles, count, param.PageIndex, param.PageSize, _contextAccessor);
            return pagination;
        }

        public async Task<IEnumerable<Role>> GetRolesNotPaginated()
        {
            var roles = await _repository.GetAllNoLimitAsync();
            return roles;
        }

        public async Task<Role> GetRoleByUniqueId(Guid uniqueId)
        {
            var role = await _repository.GetByUniqueIdAsync(uniqueId);
            if (role is null)
            {
                throw new NotFoundException($"Role with ID '{uniqueId}' not found");
            }
            return role;
        }

        public async Task<Role> GetRoleByCode(string code)
        {
            var role = await _repository.GetByCodeAsync(code);
            if (role is null)
            {
                throw new NotFoundException($"Role with Code '{code}' not found");
            }
            return role;
        }

        public async Task DeleteRole(Guid uniqueId)
        {
            var role = await _repository.GetByUniqueIdAsync(uniqueId);
            if (role is null)
            {
                throw new NotFoundException($"Role with ID '{uniqueId}' not found");
            } 
            await _repository.DeleteAsync(role);
        }

    }
}