using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyLicenseRepository : IGenericRepository<CompanyLicense>
    {
        Task<CompanyLicense?> GetLicenseByIdAsync(int licenseId);
        Task<CompanyLicense?> GetLicenseByCompanyIdAsync(int companyId);
        Task<List<CompanyLicense>> GetAllLicenseAsync();
    }
}
