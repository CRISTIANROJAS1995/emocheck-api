using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IExamSubjectService
    {
        Task<ExamSubject?> GetByIdAsync(int id);
        Task<ExamSubject?> GetByExamAsync(int examID);
        Task<List<ExamSubject>> GetAllBySubjectAsync(int subjectID);
        Task<ExamSubject?> GetByExternalExamIdAsync(string externalExamId);
        Task<ExamSubject?> GetValidAsync(int examID, int subjectID);
        Task<List<ExamSubject>> GetAllAsync();
        Task<ExamSubject> CreateAsync(ExamSubject examSubject);
        Task<ExamSubject> UpdateAsync(ExamSubject examSubject);
        Task DeleteAsync(int examSubjectId);
    }
}
