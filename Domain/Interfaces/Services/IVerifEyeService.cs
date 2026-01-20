using Domain.DTOs.VerifEye;
using Domain.Extension.Reports;
using Domain.Extension.VerifEye;

namespace Domain.Interfaces.Services
{
    public interface IVerifEyeService
    {
        Task<string> CreateExamineeAsync(
            string name,
            string email,
            string mobile,
            string token);

        Task<List<VerifEyeTestTemplate>> ListAvailableTestsAsync();

        Task<VerifEyePreparedTest> PrepareTestAsync(
            string subjectId,
            string templateId,
            Dictionary<string, string>? templateInput);

        Task<List<VerifEyeQueuedExam>> ListQueuedExamsAsync();
        Task<VerifEyeQueuedExam?> GetQueuedExamByIdAsync(string examId);
        Task<List<VerifEyeExaminee>> ListExamineesAsync();
        Task<VerifEyeExaminee?> GetExamineeByIdAsync(string subjectId);
        Task<bool> DeleteQueuedExamAsync(string examId);
        Task<List<VerifEyeExamResult>> ListExamResultsAsync();
        Task<ConverusExamReportData?> GetExamRepositoryByIdAsync(string examId);
        Task<List<QuestionDto>> GetExamQuestionsAsync(string templateId, string examId);
        Task<List<AnswerDto>> GetExamAnswersAsync(string examId);
    }
}
