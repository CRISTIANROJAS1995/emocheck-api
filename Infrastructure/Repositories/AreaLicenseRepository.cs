using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AreaLicenseRepository : GenericRepository<AreaLicense>, IAreaLicenseRepository
    {
        public AreaLicenseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<AreaLicense?> GetByAreaIdAsync(int areaID)
        {
            return await _context.AreaLicense
                .FirstOrDefaultAsync(al => al.AreaID == areaID);
        }

        public async Task<AreaLicense?> GetByAreaIdWithDetailsAsync(int areaID)
        {
            return await _context.AreaLicense
                .Include(al => al.Area)
                .Include(al => al.CompanyLicense)
                    .ThenInclude(cl => cl.Company)
                .FirstOrDefaultAsync(al => al.AreaID == areaID);
        }

        public async Task<List<AreaLicense>> GetByCompanyLicenseIdAsync(int companyLicenseID)
        {
            return await _context.AreaLicense
                .Where(al => al.CompanyLicenseID == companyLicenseID)
                .Include(al => al.Area)
                .ToListAsync();
        }

        public async Task<int> GetAvailableLicensesAsync(int areaID)
        {
            var areaLicense = await GetByAreaIdAsync(areaID);
            if (areaLicense == null) return 0;
            return areaLicense.AllocatedLicenses - areaLicense.AssignedLicenses;
        }
    }
}
