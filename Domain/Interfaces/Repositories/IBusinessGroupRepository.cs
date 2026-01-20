using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBusinessGroupRepository : IGenericRepository<BusinessGroup>
    {
        Task<List<BusinessGroup>> GetByUserIdAsync(int userId);
    }
}
