using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IAreaLicenseRepository : IGenericRepository<AreaLicense>
    {
        Task<AreaLicense> GetByAreaIdAsync(int areaID);
        Task<AreaLicense> GetByAreaIdWithDetailsAsync(int areaID);
        Task<List<AreaLicense>> GetByCompanyLicenseIdAsync(int companyLicenseID);
        Task<int> GetAvailableLicensesAsync(int areaID);
    }
}
