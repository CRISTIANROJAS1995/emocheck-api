using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IQueueService
    {
        Task<List<ExamSubject>> GetAllQueueAsync(int companyId);
        Task<bool> DeleteQueueAsync(string examId);
    }
}
