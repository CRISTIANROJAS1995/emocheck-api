using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        Task<Exam?> GetExamByIdAsync(int id);
        Task<List<Exam>> GetAllExamByTypeAsync(int examType);
        Task<Exam?> GetExamByTemplateIdAsync(string templateId);
        Task<Exam?> GetExamByTemplateIdAndCompanyAsync(string templateId, string company);
        Task<Exam?> GetExamByNameAsync(string name);
        Task<Exam?> GetExamByLocaleAsync(string locale);
        Task<List<Exam>> GetAllExamByCustomerIdAsync(string customerId);
        Task<List<Exam>> GetAllExamByCompanyAsync(string company);
        Task<List<Exam>> GetAllExamAsync();
    }
}
