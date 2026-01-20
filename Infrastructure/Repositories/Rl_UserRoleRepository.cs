using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Rl_UserRoleRepository : GenericRepository<Rl_UserRole>, IRl_UserRoleRepository
    {
        public Rl_UserRoleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Rl_UserRole>> GetAllRoleByUserAsync(int userID)
        {
            return await _context.Rl_UserRole
                .Where(r => r.UserID == userID)
                .ToListAsync();
        }

        public async Task DeleteAsync(int userID, int roleID)
        {
            var entity = await _context.Rl_UserRole
                .FirstOrDefaultAsync(x => x.UserID == userID && x.RoleID == roleID);

            if (entity != null)
            {
                _context.Rl_UserRole.Remove(entity);
                await _context.SaveChangesAsync();
            }

        }
    }
}
