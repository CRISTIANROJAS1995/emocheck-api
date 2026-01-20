using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ICatalogService
    {
        Task<List<ExamSubject>> MigrationAssignCatalogAsync();
        Task<List<Exam>> GetAllCatalogAsync(int companyId);
        Task<ExamSubject?> AssignCatalogAsync(string subjectId, string templateId, string userCreatedBy);
    }
}
