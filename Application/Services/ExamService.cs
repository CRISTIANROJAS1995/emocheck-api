using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Exceptions;

namespace Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ILogger<ExamService> _logger;

        public ExamService(IExamRepository examRepository, ILogger<ExamService> logger)
        {
            _examRepository = examRepository;
            _logger = logger;
        }

        public async Task<Exam?> GetByIdAsync(int id)
        {
            return await _examRepository.GetExamByIdAsync(id);
        }

        public async Task<List<Exam>> GetAllByTypeAsync(int examType)
        {
            return await _examRepository.GetAllExamByTypeAsync(examType);
        }

        public async Task<Exam?> GetByTemplateIdAsync(string templateId)
        {
            return await _examRepository.GetExamByTemplateIdAsync(templateId);
        }

        public async Task<Exam?> GetByTemplateIdAndCompanyAsync(string templateId, string company)
        {
            return await _examRepository.GetExamByTemplateIdAndCompanyAsync(templateId, company);
        }

        public async Task<Exam?> GetByNameAsync(string name)
        {
            return await _examRepository.GetExamByNameAsync(name);
        }

        public async Task<Exam?> GetByLocaleAsync(string locale)
        {
            return await _examRepository.GetExamByLocaleAsync(locale);
        }

        public async Task<List<Exam>> GetAllByCustomerIdAsync(string customerId)
        {
            return await _examRepository.GetAllExamByCustomerIdAsync(customerId);
        }

        public async Task<List<Exam>> GetAllByCompanyIdAsync(string company)
        {
            return await _examRepository.GetAllExamByCompanyAsync(company);
        }

        public async Task<List<Exam>> GetAllAsync()
        {
            return await _examRepository.GetAllExamAsync();
        }

        public async Task<Exam> CreateAsync(Exam exam)
        {
            var existingExam = await _examRepository.GetExamByTemplateIdAsync(exam.ExternalTemplateId);
            if (existingExam != null)
            {
                throw new EntityAlreadyExistsException("La prueba");
            }

            try
            {
                await _examRepository.AddAsync(exam);
                return exam;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la prueba: {ex.Message}");
                throw new InvalidOperationException("Error al crear la prueba", ex);
            }
        }

        public async Task UpdateAsync(Exam exam)
        {
            var existingExam = await _examRepository.GetExamByIdAsync(exam.ExamID);
            if (existingExam == null)
            {
                throw new EntityNotFoundException("La prueba");
            }

            try
            {
                existingExam.ExamTypeID = exam.ExamTypeID > 0 ? exam.ExamTypeID : existingExam.ExamTypeID;
                existingExam.ExternalTemplateId = exam.ExternalTemplateId != null ? exam.ExternalTemplateId : existingExam.ExternalTemplateId;
                existingExam.ExternalExamName = exam.ExternalExamName != null ? exam.ExternalExamName : existingExam.ExternalExamName;
                existingExam.ExternalLocale = exam.ExternalLocale != null ? exam.ExternalLocale : existingExam.ExternalLocale;

                existingExam.ExternalCustomerId = exam.ExternalCustomerId != null ? exam.ExternalCustomerId : existingExam.ExternalCustomerId;
                existingExam.Description = exam.Description != null ? exam.Description : existingExam.Description;
                existingExam.Company = exam.Company != null ? exam.Company : existingExam.Company;

                existingExam.ModifiedBy = exam.ModifiedBy != null ? exam.ModifiedBy : existingExam.ModifiedBy;
                existingExam.ModifiedAt = DateTime.Now;

                await _examRepository.UpdateAsync(existingExam);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la politica: {ex.Message}");
                throw new InvalidOperationException("Error al actualizar la politica", ex);
            }
        }
    }
}
