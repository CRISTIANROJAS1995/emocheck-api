using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Exceptions;

namespace Application.Services
{
    public class ExamSubjectService : IExamSubjectService
    {
        private readonly IExamSubjectRepository _examSubjectRepository;
        private readonly ILogger<ExamSubjectService> _logger;

        public ExamSubjectService(IExamSubjectRepository examSubjectRepository, ILogger<ExamSubjectService> logger)
        {
            _examSubjectRepository = examSubjectRepository;
            _logger = logger;
        }

        public async Task<ExamSubject?> GetByIdAsync(int id)
        {
            return await _examSubjectRepository.GetExamSubjectByIdAsync(id);
        }

        public async Task<ExamSubject?> GetByExamAsync(int examID)
        {
            return await _examSubjectRepository.GetExamSubjectByExamAsync(examID);
        }

        public async Task<List<ExamSubject>> GetAllBySubjectAsync(int subjectID)
        {
            return await _examSubjectRepository.GetAllExamSubjectBySubjectAsync(subjectID);
        }

        public async Task<ExamSubject?> GetByExternalExamIdAsync(string externalExamId)
        {
            return await _examSubjectRepository.GetExamSubjectByExternalExamIdAsync(externalExamId);
        }

        public async Task<ExamSubject?> GetValidAsync(int examID, int subjectID)
        {
            return await _examSubjectRepository.GetValidExamSubjectAsync(examID, subjectID);
        }

        public async Task<List<ExamSubject>> GetAllAsync()
        {
            return await _examSubjectRepository.GetAllExamSubjectAsync();
        }

        public async Task<ExamSubject> CreateAsync(ExamSubject examSubject)
        {
            //var existingExamSubject = await _examSubjectRepository.GetValidExamSubjectAsync(examSubject.ExamID, examSubject.SubjectID);
            //if (existingExamSubject != null)
            //{
            //    throw new EntityAlreadyExistsException("La asignación de prueba");
            //}

            try
            {
                await _examSubjectRepository.AddAsync(examSubject);
                return examSubject;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al asignar la prueba: {ex.Message}");
                throw new InvalidOperationException("Error al asignar la prueba", ex);
            }
        }

        public async Task<ExamSubject> UpdateAsync(ExamSubject examSubject)
        {
            var existingExamSubject = await _examSubjectRepository.GetExamSubjectByIdAsync(examSubject.ExamSubjectID);
            if (existingExamSubject == null)
            {
                throw new EntityNotFoundException("La asignación de prueba");
            }

            try
            {
                await _examSubjectRepository.UpdateAsync(examSubject);
                return examSubject;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la asignación de prueba: {ex.Message}");
                throw new InvalidOperationException("Error al actualizar la asignación de prueba", ex);
            }
        }

        public async Task DeleteAsync(int examSubjectId)
        {
            var existingExamSubject = await _examSubjectRepository.GetExamSubjectByIdAsync(examSubjectId);
            if (existingExamSubject == null)
            {
                throw new EntityNotFoundException("La asignación de prueba no existe");
            }

            try
            {
                await _examSubjectRepository.DeleteAsync(examSubjectId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la asignación de prueba: {ex.Message}");
                throw new InvalidOperationException("Error al eliminar la asignación de prueba", ex);
            }
        }
    }
}
