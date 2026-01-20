using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IExamService
    {
        Task<Exam?> GetByIdAsync(int id);
        Task<List<Exam>> GetAllByTypeAsync(int examType);
        Task<Exam?> GetByTemplateIdAsync(string templateId);
        Task<Exam?> GetByTemplateIdAndCompanyAsync(string templateId, string company);
        Task<Exam?> GetByNameAsync(string name);
        Task<Exam?> GetByLocaleAsync(string locale);
        Task<List<Exam>> GetAllByCustomerIdAsync(string customerId);
        Task<List<Exam>> GetAllByCompanyIdAsync(string company);
        Task<List<Exam>> GetAllAsync();
        Task<Exam> CreateAsync(Exam exam);
        Task UpdateAsync(Exam exam);
    }
}
