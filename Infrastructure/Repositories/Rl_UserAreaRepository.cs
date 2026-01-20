using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Rl_UserAreaRepository : GenericRepository<Rl_UserArea>, IRl_UserAreaRepository
    {
        public Rl_UserAreaRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Rl_UserArea>> GetAllAreaByUserAsync(int userID)
        {
            return await _context.Rl_UserArea
                .Where(r => r.UserID == userID)
                .ToListAsync();
        }

        public async Task DeleteAsync(int userID, int areaID)
        {
            var entity = await _context.Rl_UserArea
                .FirstOrDefaultAsync(x => x.UserID == userID && x.AreaID == areaID);

            if (entity != null)
            {
                _context.Rl_UserArea.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
