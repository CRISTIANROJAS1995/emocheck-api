using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Rl_UserCityRepository : GenericRepository<Rl_UserCity>, IRl_UserCityRepository
    {
        public Rl_UserCityRepository(ApplicationDbContext context) : base(context) { }

        public async Task DeleteAsync(int userID, int cityID)
        {
            var entity = await _context.Rl_UserCity
                .FirstOrDefaultAsync(x => x.UserID == userID && x.CityID == cityID);

            if (entity != null)
            {
                _context.Rl_UserCity.Remove(entity);
                await _context.SaveChangesAsync();
            }

        }
    }
}
