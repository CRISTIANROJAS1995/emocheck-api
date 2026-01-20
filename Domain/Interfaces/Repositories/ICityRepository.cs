using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<List<City>> GetCitiesByUserIdAsync(int userId);
        Task<List<City>> GetAllCitiesAsync();
    }
}
