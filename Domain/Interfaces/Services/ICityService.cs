using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ICityService
    {
        Task<List<City>> GetCitiesByUserIdAsync(int userId);
        Task<List<City>> GetAllCitiesAsync();
    }
}
