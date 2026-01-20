using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        Task<List<ExamResult>> GetAllAsync();
        Task<ExamResult?> GetByIdAsync(int id);
        Task<ExamResult?> GetByExamSubjectIdAsync(int examSubjectId, string resultExamId);
    }
}
