using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRl_UserCityRepository : IGenericRepository<Rl_UserCity>
    {
        Task DeleteAsync(int userID, int cityID);
    }
}
