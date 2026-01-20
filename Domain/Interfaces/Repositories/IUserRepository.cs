using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> ValidAuth(string email, string password);
        Task<User?> ValidActive(int userID);
        Task<List<User>> GetAllUserAsync();
        Task<User?> GetByUserIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<List<string>> GetUserRolesAsync(int userID);
    }
}
