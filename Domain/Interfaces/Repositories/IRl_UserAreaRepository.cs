using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRl_UserAreaRepository : IGenericRepository<Rl_UserArea>
    {
        Task<List<Rl_UserArea>> GetAllAreaByUserAsync(int userID);
        Task DeleteAsync(int userID, int areaID);
    }
}
