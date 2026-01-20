using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<Company?> GetCompanyByIdAsync(int companyId);
        Task<List<Company>> GetByUserIdAsync(int userId);
        Task<Company?> GetByNameAsync(string name);
        Task<List<Company>> GetAllCompanyAsync();
    }
}
