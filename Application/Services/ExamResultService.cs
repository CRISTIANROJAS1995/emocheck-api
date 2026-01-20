using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Domain.Exceptions;

namespace Application.Services
{
    public class ExamResultService : IExamResultService
    {
        private readonly IExamResultRepository _examResultRepository;
        private readonly ILogger<ExamResultService> _logger;

        public ExamResultService(
            IExamResultRepository examResultRepository,
            ILogger<ExamResultService> logger)
        {
            _examResultRepository = examResultRepository;
            _logger = logger;
        }

        public async Task<List<ExamResult>> GetAllAsync()
        {
            try
            {
                return await _examResultRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los resultados de exámenes: {ex.Message}");
                throw;
            }
        }

        public async Task<ExamResult?> GetByIdAsync(int id)
        {
            try
            {
                return await _examResultRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el resultado de examen con ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<ExamResult?> GetByExamSubjectIdAsync(int examSubjectId, string resultExamId)
        {
            try
            {
                return await _examResultRepository.GetByExamSubjectIdAsync(examSubjectId, resultExamId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el resultado de examen para ExamSubjectId {examSubjectId}: {ex.Message}");
                throw;
            }
        }

        public async Task<ExamResult> CreateAsync(ExamResult examResult)
        {
            try
            {
                await _examResultRepository.AddAsync(examResult);
                return examResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el resultado de examen: {ex.Message}");
                throw;
            }
        }

        public async Task<ExamResult> UpdateAsync(ExamResult examResult)
        {
            try
            {
                var existingResult = await _examResultRepository.GetByIdAsync(examResult.ExamResultID);
                if (existingResult == null)
                {
                    throw new EntityNotFoundException($"No se encontró el resultado de examen con ID {examResult.ExamResultID}");
                }

                await _examResultRepository.UpdateAsync(examResult);
                return examResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el resultado de examen con ID {examResult.ExamResultID}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingResult = await _examResultRepository.GetByIdAsync(id);
                if (existingResult == null)
                {
                    throw new EntityNotFoundException($"No se encontró el resultado de examen con ID {id}");
                }

                await _examResultRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el resultado de examen con ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}
