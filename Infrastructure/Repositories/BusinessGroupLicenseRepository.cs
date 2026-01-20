using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BusinessGroupLicenseRepository : GenericRepository<BusinessGroupLicense>, IBusinessGroupLicenseRepository
    {
        public BusinessGroupLicenseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<BusinessGroupLicense?> GetByBusinessGroupIdAsync(int businessGroupId)
        {
            return await _context.BusinessGroupLicense
                .Include(bgl => bgl.BusinessGroup)
                .FirstOrDefaultAsync(bgl => bgl.BusinessGroupID == businessGroupId);
        }

        public async Task<BusinessGroupLicense?> GetByBusinessGroupIdWithDetailsAsync(int businessGroupId)
        {
            return await _context.BusinessGroupLicense
                .Include(bgl => bgl.BusinessGroup)
                .FirstOrDefaultAsync(bgl => bgl.BusinessGroupID == businessGroupId);
        }

        public async Task<bool> HasSufficientLicensesAsync(int businessGroupId, int requestedQuantity)
        {
            var license = await GetByBusinessGroupIdAsync(businessGroupId);
            if (license == null) return false;
            
            var available = license.PurchasedLicenses - license.AssignedLicenses;
            return available >= requestedQuantity;
        }

        public async Task<int> GetAvailableLicensesAsync(int businessGroupId)
        {
            var license = await GetByBusinessGroupIdAsync(businessGroupId);
            return license != null ? (license.PurchasedLicenses - license.AssignedLicenses) : 0;
        }

    }
}
