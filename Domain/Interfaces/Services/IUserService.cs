using Domain.Entities;
using Domain.Extension;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Tokens> AuthenticateAsync(string email, string password);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByUserIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> CreateAsync(User user, int userCreatedBy, List<int> roles, List<int> areas);
        Task<User?> UpdateAsync(User user, int userModifiedBy, List<int>? roles = null, List<int>? areas = null);
        Task<List<Rl_UserRole>> GetAllRolesAsync(int userID);
    }
}
