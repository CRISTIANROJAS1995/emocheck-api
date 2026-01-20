using Domain.Extension.Reports;

namespace Domain.Interfaces.Services
{
    public interface IExamReportService
    {
        Task<byte[]> GenerateExamReportPdfAsync(string externalExamId);
    }
}
