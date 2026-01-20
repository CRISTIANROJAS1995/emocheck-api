using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            return await _context.Company
                .Include(c => c.BusinessGroup)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.CompanyLicenses)
                .FirstOrDefaultAsync(ua => ua.CompanyID == companyId);
        }

        public async Task<List<Company>> GetByUserIdAsync(int userId)
        {
            var companiesByAreas = _context.Rl_UserArea
                .Where(ua => ua.UserID == userId)
                .Select(ua => ua.Area.Company)
                .Distinct()
                .Include(c => c.BusinessGroup)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.CompanyLicenses);    

            return await companiesByAreas.ToListAsync();
        }

        public async Task<Company?> GetByNameAsync(string name)
        {
            return await _context.Company.FirstOrDefaultAsync(ua => ua.Name == name);
        }

        public async Task<List<Company>> GetAllCompanyAsync()
        {
            var companiesByAreas = _context.Company
                .Include(c => c.BusinessGroup)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.CompanyLicenses);

            return await companiesByAreas.ToListAsync();
        }

        public new async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Company
                .Include(c => c.BusinessGroup)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.CompanyLicenses)
                .ToListAsync();
        }
    }
}
