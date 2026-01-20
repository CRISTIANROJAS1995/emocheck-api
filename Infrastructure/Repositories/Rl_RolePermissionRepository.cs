using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Rl_RolePermissionRepository : GenericRepository<Rl_RolePermission>, IRl_RolePermissionRepository
    {
        public Rl_RolePermissionRepository(ApplicationDbContext context) : base(context) { }

        public async Task DeleteAsync(int roleID, int permissionID)
        {
            var entity = await _context.Rl_RolePermission
                .FirstOrDefaultAsync(x => x.RoleID == roleID && x.PermissionID == permissionID);

            if (entity != null)
            {
                _context.Rl_RolePermission.Remove(entity);
                await _context.SaveChangesAsync();
            }

        }
    }
}
