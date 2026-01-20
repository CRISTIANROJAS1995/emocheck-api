using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IExamResultService
    {
        Task<List<ExamResult>> GetAllAsync();
        Task<ExamResult?> GetByIdAsync(int id);
        Task<ExamResult?> GetByExamSubjectIdAsync(int examSubjectId, string resultExamId);
        Task<ExamResult> CreateAsync(ExamResult examResult);
        Task<ExamResult> UpdateAsync(ExamResult examResult);
        Task<bool> DeleteAsync(int id);
    }
}
