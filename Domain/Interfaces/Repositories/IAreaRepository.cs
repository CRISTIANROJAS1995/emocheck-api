using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        Task<List<Area>> GetByIdsAsync(List<int> areaIds);
        Task<List<Area>> GetByUserIdAsync(int userId);
    }
}
