using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICityLicenseRepository : IGenericRepository<CityLicense>
    {
        Task<CityLicense?> GetByCityIdAsync(int cityID, int areaLicenseID);
        Task<CityLicense?> GetByAreaIdAndCityIdAsync(int areaID, int cityID);
        Task<CityLicense?> GetByCityIdWithDetailsAsync(int cityID, int areaLicenseID);
        Task<List<CityLicense>> GetByAreaLicenseIdAsync(int areaLicenseID);
        Task<int> GetAvailableLicensesAsync(int cityID, int areaLicenseID);
    }
}
