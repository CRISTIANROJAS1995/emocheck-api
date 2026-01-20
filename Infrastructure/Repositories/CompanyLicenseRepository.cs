using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyLicenseRepository : GenericRepository<CompanyLicense>, ICompanyLicenseRepository
    {
        public CompanyLicenseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<CompanyLicense?> GetLicenseByIdAsync(int licenseId)
        {
            return await _context.CompanyLicense
                .Include(c => c.Company)
                .Include(c => c.Company.BusinessGroup)
                .Include(c => c.Company.Country)
                .Include(c => c.Company.State)
                .Include(c => c.Company.CompanyLicenses)
                .FirstOrDefaultAsync(ua => ua.CompanyLicenseID == licenseId);
        }

        public async Task<CompanyLicense?> GetLicenseByCompanyIdAsync(int companyId)
        {
            return await _context.CompanyLicense
                .Include(c => c.Company)
                .Include(c => c.Company.BusinessGroup)
                .Include(c => c.Company.Country)
                .Include(c => c.Company.State)
                .Include(c => c.Company.CompanyLicenses)
                .FirstOrDefaultAsync(ua => ua.CompanyID == companyId);
        }

        public async Task<List<CompanyLicense>> GetAllLicenseAsync()
        {
            var licenses = _context.CompanyLicense
                .Include(c => c.Company)
                .Include(c => c.Company.BusinessGroup)
                .Include(c => c.Company.Country)
                .Include(c => c.Company.State)
                .Include(c => c.Company.CompanyLicenses);

            return await licenses.ToListAsync();
        }

    }
}
