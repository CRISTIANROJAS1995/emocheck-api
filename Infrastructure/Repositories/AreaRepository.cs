using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AreaRepository : GenericRepository<Area>, IAreaRepository
    {
        public AreaRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Area>> GetByIdsAsync(List<int> areaIds)
        {
            return await _context.Area
                .Where(a => areaIds.Contains(a.AreaID))
                .ToListAsync();
        }

        public async Task<List<Area>> GetByUserIdAsync(int userId)
        {
            return await _context.Rl_UserArea
                .Where(ua => ua.UserID == userId)
                .Include(ua => ua.Area)
                    .ThenInclude(a => a.Company)
                        .ThenInclude(c => c.Country)
                .Include(ua => ua.Area)
                    .ThenInclude(a => a.Company)
                        .ThenInclude(c => c.State)
                .Include(ua => ua.Area)
                    .ThenInclude(a => a.Company)
                        .ThenInclude(c => c.BusinessGroup)
                .Select(ua => ua.Area)
                .Distinct()
                .ToListAsync();
        }
    }
}
