using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ISubjectService
    {
        Task<Subject?> GetByIdentifierAsync(string identifier);
        Task<List<Subject>> GetAllByCompanyAsync(int companyID);
        Task<Subject?> GetByNameAsync(string name);
        Task<Subject?> GetByIdExternalAsync(string idExternal);
        Task<List<Subject>> GetAllAsync(int companyId);
        Task<List<Subject>> GetAllSubjectAsync();
        Task<Subject?> CreateAsync(Subject subject, int userCreatedBy);
        Task<List<Subject>> MigrationOfConverusAsync();
    }
}
