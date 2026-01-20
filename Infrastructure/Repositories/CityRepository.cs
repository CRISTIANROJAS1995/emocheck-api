using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<City>> GetCitiesByUserIdAsync(int userId)
        {
            return await _context.Rl_UserCity
                .Where(rc => rc.UserID == userId)
                .Include(rc => rc.City)
                .Select(rc => rc.City)
                .ToListAsync();
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _context.City.ToListAsync();
        }
    }
}
