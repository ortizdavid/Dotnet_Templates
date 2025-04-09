using TemplateMongoDbApi.Common.Exceptions;
using TemplateMongoDbApi.Common.Helpers;
using TemplateMongoDbApi.Core.DTOs.Auth;
using TemplateMongoDbApi.Core.Mappers.Auth;
using TemplateMongoDbApi.Core.Models.Auth;
using TemplateMongoDbApi.Core.Repositories.Auth;

namespace TemplateMongoDbApi.Core.Services.Auth;

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

    public async Task UpdateRole(RoleRequest request, string roleId)
    {
        if (request is null)
        {
            throw new BadRequestException("Please provide role name and code");
        }
        var role = await _repository.GetByIdAsync(roleId);
        if (role is null)
        {
            throw new NotFoundException($"Role with ID '{roleId}' not found");
        }
        role.RoleName = request.RoleName;
        role.Code = request.Code;
        await _repository.UpdateAsync(role, roleId); 
    }

    public async Task<Pagination<RoleResponse>> GetAllRoles(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var roles = await _repository.GetAllAsync(param.PageSize, param.PageIndex);
        var roleResponses = RoleMapper.ToResponseList(roles);
        var pagination = new Pagination<RoleResponse>(roleResponses, count, param.PageIndex, param.PageSize, _contextAccessor);
        return pagination;
    }

    public async Task<IEnumerable<RoleResponse>> GetRolesNotPaginated()
    {
        var roles = await _repository.GetAllNotPaginatedAsync();
        var roleResponses = RoleMapper.ToResponseList(roles);
        return roleResponses;
    }

    public async Task<RoleResponse> GetRoleById(string roleId)
    {
        var role = await _repository.GetByIdAsync(roleId);
        if (role is null)
        {
            throw new NotFoundException($"Role with ID '{roleId}' not found");
        }
        var roleResponse = RoleMapper.ToResponse(role);
        return roleResponse;
    }

    public async Task<RoleResponse> GetRoleByCode(string code)
    {
        var role = await _repository.GetByCodeAsync(code);
        if (role is null)
        {
            throw new NotFoundException($"Role with Code '{code}' not found");
        }
        var roleResponse = RoleMapper.ToResponse(role);
        return roleResponse;
    }

    public async Task DeleteRole(string roleId)
    {
        var role = await _repository.GetByIdAsync(roleId);
        if (role is null)
        {
            throw new NotFoundException($"Role with ID '{roleId}' not found");
        } 
        await _repository.DeleteAsync(roleId);
    }
}