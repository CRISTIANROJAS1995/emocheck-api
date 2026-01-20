using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LicenseLogRepository : GenericRepository<LicenseLog>, ILicenseLogRepository
    {
        public LicenseLogRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<LicenseLog>> GetByLevelAndEntityIdAsync(string licenseLevel, int entityID)
        {
            return await _context.LicenseLog
                .Where(ll => ll.LicenseLevel == licenseLevel && ll.EntityID == entityID)
                .OrderByDescending(ll => ll.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<LicenseLog>> GetByBusinessGroupIdAsync(int businessGroupID)
        {
            return await GetByLevelAndEntityIdAsync("BusinessGroup", businessGroupID);
        }

        public async Task<List<LicenseLog>> GetByCompanyIdAsync(int companyID)
        {
            return await GetByLevelAndEntityIdAsync("Company", companyID);
        }

        public async Task<List<LicenseLog>> GetByAreaIdAsync(int areaID)
        {
            return await GetByLevelAndEntityIdAsync("Area", areaID);
        }

        public async Task<List<LicenseLog>> GetByCityIdAsync(int cityID)
        {
            return await GetByLevelAndEntityIdAsync("City", cityID);
        }

        public async Task<List<LicenseLog>> GetByActionTypeAsync(string actionType)
        {
            return await _context.LicenseLog
                .Where(ll => ll.ActionType == actionType)
                .OrderByDescending(ll => ll.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<LicenseLog>> GetByLicenseIdAsync(int licenseID)
        {
            return await _context.LicenseLog
                .Where(ll => ll.LicenseID == licenseID)
                .OrderByDescending(ll => ll.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<LicenseLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.LicenseLog
                .Where(ll => ll.CreatedAt >= startDate && ll.CreatedAt <= endDate)
                .OrderByDescending(ll => ll.CreatedAt)
                .ToListAsync();
        }
    }
}
