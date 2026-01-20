using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ILicenseLogRepository : IGenericRepository<LicenseLog>
    {
        Task<List<LicenseLog>> GetByLevelAndEntityIdAsync(string licenseLevel, int entityID);
        Task<List<LicenseLog>> GetByBusinessGroupIdAsync(int businessGroupID);
        Task<List<LicenseLog>> GetByCompanyIdAsync(int companyID);
        Task<List<LicenseLog>> GetByAreaIdAsync(int areaID);
        Task<List<LicenseLog>> GetByCityIdAsync(int cityID);
        Task<List<LicenseLog>> GetByActionTypeAsync(string actionType);
        Task<List<LicenseLog>> GetByLicenseIdAsync(int licenseID);
        Task<List<LicenseLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
