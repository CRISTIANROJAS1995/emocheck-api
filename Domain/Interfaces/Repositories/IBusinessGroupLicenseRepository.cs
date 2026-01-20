using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBusinessGroupLicenseRepository : IGenericRepository<BusinessGroupLicense>
    {
        Task<BusinessGroupLicense> GetByBusinessGroupIdAsync(int businessGroupID);
        Task<BusinessGroupLicense> GetByBusinessGroupIdWithDetailsAsync(int businessGroupID);
        Task<bool> HasSufficientLicensesAsync(int businessGroupID, int requestedQuantity);
        Task<int> GetAvailableLicensesAsync(int businessGroupID);
    }
}
