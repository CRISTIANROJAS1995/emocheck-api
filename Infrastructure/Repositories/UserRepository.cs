using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> ValidAuth(string email, string password)
        {
            return await _context.User
                .Include(u => u.State)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.CompanyLicenses)

                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.BusinessGroup)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.Country)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.State)
                .Include(u => u.UserCities)
                    .ThenInclude(uc => uc.City)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<User?> ValidActive(int userID)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.UserID == userID && u.StateID == (int)StateEnum.Active);
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _context.User
                .Include(u => u.State)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.CompanyLicenses)

                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.BusinessGroup)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.Country)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.State)
                .Include(u => u.UserCities)
                    .ThenInclude(uc => uc.City)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<User?> GetByUserIdAsync(int userId)
        {
            return await _context.User
                .Include(u => u.State)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.CompanyLicenses)

                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.BusinessGroup)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.Country)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.State)
                .Include(u => u.UserCities)
                    .ThenInclude(uc => uc.City)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.UserID == userId);

        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.User
                .Include(u => u.State)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.CompanyLicenses)

                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.BusinessGroup)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.Country)
                .Include(u => u.UserAreas)
                    .ThenInclude(ua => ua.Area)
                        .ThenInclude(a => a.Company)
                            .ThenInclude(c => c.State)
                .Include(u => u.UserCities)
                    .ThenInclude(uc => uc.City)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<List<string>> GetUserRolesAsync(int userID)
        {
            return await _context.Rl_UserRole
                .Include(ur => ur.Role)
                .Where(ur => ur.UserID == userID)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }
    }
}
