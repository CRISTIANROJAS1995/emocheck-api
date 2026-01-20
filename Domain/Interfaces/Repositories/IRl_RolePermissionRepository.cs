using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRl_RolePermissionRepository : IGenericRepository<Rl_RolePermission>
    {
        Task DeleteAsync(int roleID, int permissionID);
    }
}
