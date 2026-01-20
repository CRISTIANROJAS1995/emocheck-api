using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ILicenseConfigurationRepository : IGenericRepository<LicenseConfiguration>
    {
        Task<LicenseConfiguration> GetByKeyAsync(string configKey);
        Task<int> GetMaxOverdraftLicensesAsync();
        Task UpdateConfigValueAsync(string configKey, int newValue, string modifiedBy);
    }
}
