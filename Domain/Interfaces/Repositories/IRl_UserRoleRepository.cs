using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRl_UserRoleRepository : IGenericRepository<Rl_UserRole>
    {
        Task<List<Rl_UserRole>> GetAllRoleByUserAsync(int userID);
        Task DeleteAsync(int userID, int roleID);
    }
}
