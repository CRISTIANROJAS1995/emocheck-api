using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IResultService
    {
        Task<List<ExamResult>> MigrateResultsAsync();
        Task<List<ExamResult>> GetAllResultsAsync(int companyId);
        Task<byte[]> GenerateExamReportPdfAsync(int examResultId);
    }
}
