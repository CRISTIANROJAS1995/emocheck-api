using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IAreaService
    {
        Task<List<Area>> GetByUserIdAsync(int userId);
        Task<List<Area>> GetByIdsAsync(List<int> areaIds);
    }
}
