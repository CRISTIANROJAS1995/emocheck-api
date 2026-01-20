using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class QueueService : IQueueService
    {
        private readonly IExamService _examService;
        private readonly IExamSubjectService _examSubjectService;
        private readonly IVerifEyeService _verifEyeService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CatalogService> _logger;

        public QueueService(IExamService examService, IExamSubjectService examSubjectService, IVerifEyeService verifEyeService, ICompanyRepository companyRepository, ILogger<CatalogService> logger)
        {
            _examService = examService;
            _examSubjectService = examSubjectService;
            _verifEyeService = verifEyeService;
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<List<ExamSubject>> GetAllQueueAsync(int companyId)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(companyId);
            if (existingCompany == null)
            {
                throw new ArgumentException("El cliente enviado no existe");
            }

            var allAssignments = await _examSubjectService.GetAllAsync();
            var filtered = allAssignments
                .Where(x => x.Exam.Company == existingCompany.Name)
                .ToList();

            // Creamos una lista para los que sí existen en Converus
            var validAssignments = new List<ExamSubject>();

            foreach (var examSubject in filtered)
            {
                var queuedExam = await _verifEyeService.GetQueuedExamByIdAsync(examSubject.ExternalExamId);
                if (queuedExam != null)
                {
                    bool requiereActualizacion =
                        examSubject.ExternalExamUrl != queuedExam.ExamUrl ||
                        examSubject.ExternalExamQueued != queuedExam.ExamQueued.ToString("O") ||
                        examSubject.ExternalExamStatus != queuedExam.ExamStatus ||
                        examSubject.ExternalExamStep != queuedExam.ExamStep;

                    if (requiereActualizacion)
                    {
                        examSubject.ExternalExamUrl = queuedExam.ExamUrl;
                        examSubject.ExternalExamQueued = queuedExam.ExamQueued.ToString("O");
                        examSubject.ExternalExamStatus = queuedExam.ExamStatus;
                        examSubject.ExternalExamStep = queuedExam.ExamStep;
                        examSubject.ModifiedAt = DateTime.UtcNow;
                        examSubject.ModifiedBy = "system";

                        await _examSubjectService.UpdateAsync(examSubject);
                    }
                    validAssignments.Add(examSubject);
                }
            }

            return validAssignments;
        }

        public async Task<bool> DeleteQueueAsync(string examId)
        {
            // Elimina en Converus
            var deleted = await _verifEyeService.DeleteQueuedExamAsync(examId);

            if (deleted)
            {
                _logger.LogInformation("Examen {ExamId} eliminado correctamente en Converus.", examId);
            }
            else
            {
                _logger.LogWarning("No se pudo eliminar el examen {ExamId} en Converus.");
            }

            return deleted;
        }
    }
}
