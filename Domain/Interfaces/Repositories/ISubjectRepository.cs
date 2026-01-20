using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<Subject?> GetSubjectByIdAsync(int id);
        Task<Subject?> GetSubjectByIdentifierAsync(string identifier);
        Task<List<Subject>> GetAllSubjectByCompanyAsync(int companyID);
        Task<Subject?> GetSubjectByNameAsync(string name);
        Task<Subject?> GetSubjectByIdExternalAsync(string idExternal);
        Task<List<Subject>> GetAllSubjectAsync();
        Task<Subject?> GetValidExistAsync(string identification, int companyID);
    }
}
