using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IExamSubjectRepository : IGenericRepository<ExamSubject>
    {
        Task<ExamSubject?> GetExamSubjectByIdAsync(int id);
        Task<ExamSubject?> GetExamSubjectByExamAsync(int examID);
        Task<List<ExamSubject>> GetAllExamSubjectBySubjectAsync(int subjectID);
        Task<ExamSubject?> GetExamSubjectByExternalExamIdAsync(string externalExamId);
        Task<ExamSubject?> GetValidExamSubjectAsync(int examID, int subjectID);
        Task<List<ExamSubject>> GetAllExamSubjectAsync();
    }
}
