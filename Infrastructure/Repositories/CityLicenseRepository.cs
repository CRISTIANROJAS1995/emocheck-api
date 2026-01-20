using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CityLicenseRepository : GenericRepository<CityLicense>, ICityLicenseRepository
    {
        public CityLicenseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<CityLicense?> GetByCityIdAsync(int cityID, int areaLicenseID)
        {
            return await _context.CityLicense
                .FirstOrDefaultAsync(cl => cl.CityID == cityID && cl.AreaLicenseID == areaLicenseID);
        }

        public async Task<CityLicense?> GetByAreaIdAndCityIdAsync(int areaID, int cityID)
        {
            return await _context.CityLicense
                .Include(cl => cl.AreaLicense)
                .FirstOrDefaultAsync(cl => cl.CityID == cityID && cl.AreaLicense.AreaID == areaID);
        }

        public async Task<CityLicense?> GetByCityIdWithDetailsAsync(int cityID, int areaLicenseID)
        {
            return await _context.CityLicense
                .Include(cl => cl.City)
                .Include(cl => cl.AreaLicense)
                    .ThenInclude(al => al.Area)
                .FirstOrDefaultAsync(cl => cl.CityID == cityID && cl.AreaLicenseID == areaLicenseID);
        }

        public async Task<List<CityLicense>> GetByAreaLicenseIdAsync(int areaLicenseID)
        {
            return await _context.CityLicense
                .Where(cl => cl.AreaLicenseID == areaLicenseID)
                .Include(cl => cl.City)
                .ToListAsync();
        }

        public async Task<int> GetAvailableLicensesAsync(int cityID, int areaLicenseID)
        {
            var cityLicense = await GetByCityIdAsync(cityID, areaLicenseID);
            if (cityLicense == null) return 0;
            return cityLicense.AllocatedLicenses - cityLicense.AssignedLicenses - cityLicense.UsedLicenses;
        }
    }
}
