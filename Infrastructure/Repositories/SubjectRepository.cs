using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{

    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .FirstOrDefaultAsync(u => u.SubjectID == id);
        }


        public async Task<Subject?> GetSubjectByIdentifierAsync(string identifier)
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .FirstOrDefaultAsync( u => u.Identifier == identifier);
        }

        public async Task<List<Subject>> GetAllSubjectByCompanyAsync(int companyID)
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .Where(u => u.CompanyID == companyID)
                .ToListAsync();
        }

        public async Task<Subject?> GetSubjectByNameAsync(string name)
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<Subject?> GetSubjectByIdExternalAsync(string idExternal)
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .FirstOrDefaultAsync(u => u.IdentifierExternal == idExternal);
        }

        public async Task<List<Subject>> GetAllSubjectAsync()
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .ToListAsync();
        }

        public async Task<Subject?> GetValidExistAsync(string identification, int companyID)
        {
            return await _context.Subject
                .Include(u => u.Company)
                .Include(u => u.Company.BusinessGroup)
                .Include(u => u.Company.Country)
                .Include(u => u.Company.State)
                .FirstOrDefaultAsync(u => u.Identifier == identification && u.CompanyID == companyID);
        }
    }
}
