using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<Company?> GetCompanyByIdAsync(int companyId);
        Task<List<Company>> GetAllAsync();
        Task<List<Company>> GetByUserIdAsync(int userId);
        Task<Company> CreateAsync(Company company);
        Task UpdateAsync(Company company);
        Task<List<Company>> MigrationOfConverusAsync();

        // License management methods - Simplified
        Task<bool> AssignLicensesToCompanyAsync(int companyId, int totalLicenses, string assignedBy);
        Task<bool> ConsumeLicenseAsync(int companyId, int examResultId, string consumedBy);
        Task<CompanyLicense?> GetCompanyLicensesAsync(int companyId);
        Task<List<CompanyLicense>> GetAllCompanyLicensesAsync();
        Task<int> GetAvailableLicensesAsync(int companyId);
    }
}
