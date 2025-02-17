using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Auth;
using TemplateMVC.Core.Repositories.Auth;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Services.Auth;

public class RoleService
{
    private readonly RoleRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;

    public RoleService(RoleRepository repository, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _contextAccessor = contextAccessor;
    }

    public async Task CreateRole(CreateRoleViewModel viewModel)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Please provide role name and code");
        }
        if (await _repository.ExistsRecord("RoleName", viewModel.RoleName))
        {
            throw new ConflictException($"Role '{viewModel.RoleName}' already exists");
        }
        if (await _repository.ExistsRecord("Code", viewModel.Code))
        {
            throw new ConflictException($"Code '{viewModel.Code}' already exists");
        }
        var role = new Role()
        {
            RoleName = viewModel.RoleName,
            Code = viewModel.Code,
        };
        await _repository.CreateAsync(role);
    }

    public async Task UpdateRole(UpdateRoleViewModel viewModel, Guid uniqueId)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("Please provide role name and code");
        }
        var role = await _repository.GetByUniqueIdAsync(uniqueId);
        if (role is null)
        {
            throw new NotFoundException($"Role with ID '{uniqueId}' not found");
        }
        if (await _repository.ExistsRecordExcluded(viewModel.RoleName, viewModel.Code, uniqueId))
        {
            throw new ConflictException("Role Name or Code already exists");
        }
        role.RoleName = viewModel.RoleName;
        role.Code = viewModel.Code;
        await _repository.UpdateAsync(role); 
    }

    public async Task<Pagination<Role>> GetAllRoles(PaginationParam param, SearchFilter filter)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var roles = await _repository.GetAllSortedAsync(param.PageSize, param.PageIndex, filter.SearchString, filter.SortOrder);
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