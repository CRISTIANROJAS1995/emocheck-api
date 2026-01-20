using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BusinessGroupRepository : GenericRepository<BusinessGroup>, IBusinessGroupRepository
    {
        public BusinessGroupRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<BusinessGroup>> GetByUserIdAsync(int userId)
        {
            var businessGroups = await _context.Rl_UserArea
                .Where(ua => ua.UserID == userId)
                .Include(ua => ua.Area)
                    .ThenInclude(a => a.Company)
                        .ThenInclude(c => c.BusinessGroup)
                .Include(ua => ua.Area)
                    .ThenInclude(a => a.Company)
                        .ThenInclude(c => c.Country)
                .Include(ua => ua.Area)
                    .ThenInclude(a => a.Company)
                        .ThenInclude(c => c.State)
                .Select(ua => ua.Area.Company.BusinessGroup)
                .Distinct()
                .ToListAsync();

            return businessGroups;
        }
    }
}
