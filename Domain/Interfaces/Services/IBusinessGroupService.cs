using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IBusinessGroupService
    {
        Task<List<BusinessGroup>> GetByUserIdAsync(int userId);
    }
}
