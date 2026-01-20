using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Domain.Extension.Reports;
using Domain.Extension.VerifEye;

namespace Application.Services
{
    public class ResultService : IResultService
    {
        private readonly IExamResultService _examResultService;
        private readonly IExamSubjectService _examSubjectService;
        private readonly IVerifEyeService _verifEyeService;
        private readonly IExamReportService _examReportService;
        private readonly IExamService _examService;
        private readonly ISubjectService _subjectService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILicenseManagementService _licenseManagementService;
        private readonly ILogger<ResultService> _logger;

        public ResultService(
            IExamResultService examResultService,
            IExamSubjectService examSubjectService,
            IVerifEyeService verifEyeService,
            IExamReportService examReportService,
            IExamService examService,
            ISubjectService subjectService,
            ICompanyRepository companyRepository,
            ILicenseManagementService licenseManagementService,
            ILogger<ResultService> logger)
        {
            _examResultService = examResultService;
            _examSubjectService = examSubjectService;
            _verifEyeService = verifEyeService;
            _examReportService = examReportService;
            _examService = examService;
            _subjectService = subjectService;
            _companyRepository = companyRepository;
            _licenseManagementService = licenseManagementService;
            _logger = logger;
        }

        public async Task<List<ExamResult>> MigrateResultsAsync()
        {
            var verifEyeResults = await _verifEyeService.ListExamResultsAsync();
            if (verifEyeResults.Count > 0)
            {
                foreach (VerifEyeExamResult item in verifEyeResults)
                {
                    //Consultamos la asignación
                    var examSubject = await _examSubjectService.GetByExternalExamIdAsync(item.ExamId);
                    if (examSubject == null)
                    {
                        continue;
                    }
                    else
                    {
                        var valid = await _examResultService.GetByExamSubjectIdAsync(examSubject.ExamSubjectID, item.ExamId);
                        if (valid == null)
                        {
                            var examResult = new ExamResult(
                                examSubjectID: examSubject.ExamSubjectID,
                                externalExamErrors: item.ExamErrors,
                                externalExamModel: item.ExamModel,
                                externalExamQuestions: item.ExamQuestions,
                                externalExamQueued: item.ExamQueued,
                                externalExamResult1: item.ExamResult1,
                                externalExamResult2: item.ExamResult2,
                                externalExamResult3: item.ExamResult3,
                                externalExamScore1: item.ExamScore1,
                                externalExamScore2: item.ExamScore2,
                                externalExamScore3: item.ExamScore3,
                                externalExamScore4: item.ExamScore4,
                                externalExamScored: item.ExamScored,
                                externalExamTimeouts: item.ExamTimeouts,
                                externalExamTopic: item.ExamTopic,
                                externalTemplateType: item.TemplateType,
                                resultExamId: item.ExamId
                            );

                            await _examResultService.CreateAsync(examResult);
                            try
                            {
                                await _licenseManagementService.MarkLicenseAsUsedAsync(
                                    examSubject.ExamSubjectID,
                                    examResult.ExamResultID,
                                    "system"
                                );
                                _logger.LogInformation($"Licencia marcada como usada para ExamSubject {examSubject.ExamSubjectID}");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"No se pudo marcar la licencia como usada para ExamSubject {examSubject.ExamSubjectID}: {ex.Message}");
                                // No falla el proceso completo si no se puede marcar la licencia
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            return await _examResultService.GetAllAsync();
        }

        public async Task<List<ExamResult>> GetAllResultsAsync(int companyId)
        {
            var validResults = new List<ExamResult>();

            var existingCompany = await _companyRepository.GetByIdAsync(companyId);
            if (existingCompany == null)
            {
                throw new ArgumentException("El cliente enviado no existe");
            }

            var verifEyeResults = await _verifEyeService.ListExamResultsAsync();
            if (verifEyeResults.Count > 0)
            {
                foreach (VerifEyeExamResult verifEyeResult in verifEyeResults)
                {
                    var consultSubject = await _subjectService.GetByIdExternalAsync(verifEyeResult.SubjectId);
                    if (consultSubject == null)
                    {
                        continue;
                    }

                    var consultExam = await _examService.GetByTemplateIdAndCompanyAsync(verifEyeResult.TemplateId, existingCompany.Name);
                    if (consultExam == null)
                    {
                        continue;
                    }

                    var consultExamSubject = await _examSubjectService.GetValidAsync(consultExam.ExamID, consultSubject.SubjectID);
                    if (consultExamSubject == null)
                    {
                        continue;
                    }

                    var consultExamResult = await _examResultService.GetByExamSubjectIdAsync(consultExamSubject!.ExamSubjectID, verifEyeResult.ExamId);
                    if (consultExamResult == null)
                    {
                        var newResult = new ExamResult(
                            examSubjectID: consultExamSubject.ExamSubjectID,
                            externalExamErrors: verifEyeResult.ExamErrors!,
                            externalExamModel: verifEyeResult.ExamModel!,
                            externalExamQuestions: verifEyeResult.ExamQuestions!,
                            externalExamQueued: verifEyeResult.ExamQueued!,
                            externalExamResult1: verifEyeResult.ExamResult1!,
                            externalExamResult2: verifEyeResult.ExamResult2!,
                            externalExamResult3: verifEyeResult.ExamResult3!,
                            externalExamScore1: verifEyeResult.ExamScore1!,
                            externalExamScore2: verifEyeResult.ExamScore2!,
                            externalExamScore3: verifEyeResult.ExamScore3!,
                            externalExamScore4: verifEyeResult.ExamScore4!,
                            externalExamScored: verifEyeResult.ExamScored!,
                            externalExamTimeouts: verifEyeResult.ExamTimeouts!,
                            externalExamTopic: verifEyeResult.ExamTopic!,
                            externalTemplateType: verifEyeResult.TemplateType!,
                            resultExamId: verifEyeResult.ExamId);

                        await _examResultService.CreateAsync(newResult);

                        // Marcar la licencia como usada
                        try
                        {
                            await _licenseManagementService.MarkLicenseAsUsedAsync(
                                consultExamSubject.ExamSubjectID,
                                newResult.ExamResultID,
                                "system"
                            );
                            _logger.LogInformation($"Licencia marcada como usada para ExamSubject {consultExamSubject.ExamSubjectID} en GetAllResultsAsync");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"No se pudo marcar la licencia como usada para ExamSubject {consultExamSubject.ExamSubjectID}: {ex.Message}");
                        }
                    }
                    else
                    {
                        // Verificar si los datos han cambiado en Converus y actualizar si es necesario
                        bool hasChanges = HasResultChanged(consultExamResult, verifEyeResult);
                        if (hasChanges)
                        {
                            // Actualizar el resultado existente con los nuevos datos de Converus
                            consultExamResult.ExternalExamErrors = verifEyeResult.ExamErrors ?? string.Empty;
                            consultExamResult.ExternalExamModel = verifEyeResult.ExamModel ?? string.Empty;
                            consultExamResult.ExternalExamQuestions = verifEyeResult.ExamQuestions ?? string.Empty;
                            consultExamResult.ExternalExamQueued = verifEyeResult.ExamQueued;
                            consultExamResult.ExternalExamResult1 = verifEyeResult.ExamResult1 ?? string.Empty;
                            consultExamResult.ExternalExamResult2 = verifEyeResult.ExamResult2 ?? string.Empty;
                            consultExamResult.ExternalExamResult3 = verifEyeResult.ExamResult3 ?? string.Empty;
                            consultExamResult.ExternalExamScore1 = verifEyeResult.ExamScore1 ?? string.Empty;
                            consultExamResult.ExternalExamScore2 = verifEyeResult.ExamScore2 ?? string.Empty;
                            consultExamResult.ExternalExamScore3 = verifEyeResult.ExamScore3 ?? string.Empty;
                            consultExamResult.ExternalExamScore4 = verifEyeResult.ExamScore4 ?? string.Empty;
                            consultExamResult.ExternalExamScored = verifEyeResult.ExamScored;
                            consultExamResult.ExternalExamTimeouts = verifEyeResult.ExamTimeouts ?? string.Empty;
                            consultExamResult.ExternalExamTopic = verifEyeResult.ExamTopic ?? string.Empty;
                            consultExamResult.ExternalTemplateType = verifEyeResult.TemplateType ?? string.Empty;

                            await _examResultService.UpdateAsync(consultExamResult);
                        }
                    }
                }
            }

            return await _examResultService.GetAllAsync();
        }

        public async Task<byte[]> GenerateExamReportPdfAsync(int examResultId)
        {
            var examResult = await _examResultService.GetByIdAsync(examResultId);
            if (examResult == null)
            {
                throw new ArgumentException("El resultado del examen no existe");
            }

            // Consumir el nuevo servicio usando el externalExamId
            var externalExamId = examResult.ExamSubject?.ExternalExamId;
            if (string.IsNullOrEmpty(externalExamId))
            {
                throw new ArgumentException("El resultado del examen no tiene ExternalExamId");
            }
            return await _examReportService.GenerateExamReportPdfAsync(externalExamId);
        }

        private bool HasResultChanged(ExamResult existingResult, dynamic verifEyeResult)
        {
            // Comparar todos los campos relevantes para detectar cambios
            return existingResult.ExternalExamErrors != (verifEyeResult.ExamErrors ?? string.Empty) ||
                   existingResult.ExternalExamModel != (verifEyeResult.ExamModel ?? string.Empty) ||
                   existingResult.ExternalExamQuestions != (verifEyeResult.ExamQuestions ?? string.Empty) ||
                   existingResult.ExternalExamQueued != verifEyeResult.ExamQueued ||
                   existingResult.ExternalExamResult1 != (verifEyeResult.ExamResult1 ?? string.Empty) ||
                   existingResult.ExternalExamResult2 != (verifEyeResult.ExamResult2 ?? string.Empty) ||
                   existingResult.ExternalExamResult3 != (verifEyeResult.ExamResult3 ?? string.Empty) ||
                   existingResult.ExternalExamScore1 != (verifEyeResult.ExamScore1 ?? string.Empty) ||
                   existingResult.ExternalExamScore2 != (verifEyeResult.ExamScore2 ?? string.Empty) ||
                   existingResult.ExternalExamScore3 != (verifEyeResult.ExamScore3 ?? string.Empty) ||
                   existingResult.ExternalExamScore4 != (verifEyeResult.ExamScore4 ?? string.Empty) ||
                   existingResult.ExternalExamScored != verifEyeResult.ExamScored ||
                   existingResult.ExternalExamTimeouts != (verifEyeResult.ExamTimeouts ?? string.Empty) ||
                   existingResult.ExternalExamTopic != (verifEyeResult.ExamTopic ?? string.Empty) ||
                   existingResult.ExternalTemplateType != (verifEyeResult.TemplateType ?? string.Empty);
        }

        private string GenerateConclusion(ExamResult examResult)
        {
            // Lógica para generar la conclusión basada en los resultados
            if (!string.IsNullOrEmpty(examResult.ExternalExamResult1))
            {
                return $"El examinado fue categorizado como '{examResult.ExternalExamResult1}' basado en los resultados del análisis.";
            }

            return "El examinado completó el proceso de evaluación. Se requiere revisión manual para determinar la categorización final.";
        }

        private int CalculateDataQuality(ExamResult examResult)
        {
            // Lógica para calcular la calidad de datos
            int totalFields = 10;
            int validFields = 0;

            if (!string.IsNullOrEmpty(examResult.ExternalExamResult1)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamResult2)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamResult3)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamScore1)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamScore2)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamScore3)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamScore4)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamTopic)) validFields++;
            if (!string.IsNullOrEmpty(examResult.ExternalExamModel)) validFields++;
            if (examResult.ExternalExamScored.HasValue) validFields++;

            return (int)((double)validFields / totalFields * 100);
        }

        private List<ExamResultTopic> MapExamResultsToTopics(ExamResult examResult)
        {
            var topics = new List<ExamResultTopic>();

            if (!string.IsNullOrEmpty(examResult.ExternalExamTopic) && !string.IsNullOrEmpty(examResult.ExternalExamScore1))
            {
                topics.Add(new ExamResultTopic
                {
                    Topic = examResult.ExternalExamTopic,
                    Score = examResult.ExternalExamScore1,
                    Status = DetermineStatus(examResult.ExternalExamScore1)
                });
            }

            if (!string.IsNullOrEmpty(examResult.ExternalExamScore2))
            {
                topics.Add(new ExamResultTopic
                {
                    Topic = "Categoría 2",
                    Score = examResult.ExternalExamScore2,
                    Status = DetermineStatus(examResult.ExternalExamScore2)
                });
            }

            if (!string.IsNullOrEmpty(examResult.ExternalExamScore3))
            {
                topics.Add(new ExamResultTopic
                {
                    Topic = "Categoría 3",
                    Score = examResult.ExternalExamScore3,
                    Status = DetermineStatus(examResult.ExternalExamScore3)
                });
            }

            if (!string.IsNullOrEmpty(examResult.ExternalExamScore4))
            {
                topics.Add(new ExamResultTopic
                {
                    Topic = "Categoría 4",
                    Score = examResult.ExternalExamScore4,
                    Status = DetermineStatus(examResult.ExternalExamScore4)
                });
            }

            // Si no hay temas específicos, agregar algunos por defecto
            if (topics.Count == 0)
            {
                topics.AddRange(new List<ExamResultTopic>
                {
                    new ExamResultTopic { Topic = "Falsificación de documentos", Score = "N/A", Status = "N/A" },
                    new ExamResultTopic { Topic = "Vínculos con grupos delictivos", Score = "N/A", Status = "N/A" },
                    new ExamResultTopic { Topic = "Comportamiento ético", Score = "N/A", Status = "N/A" }
                });
            }

            return topics;
        }

        private string DetermineStatus(string score)
        {
            // Lógica para determinar el estado basado en el puntaje
            if (string.IsNullOrEmpty(score) || score == "N/A")
                return "N/A";

            // Extraer el número del puntaje (ej: "93.59%" -> 93.59)
            var numericScore = ExtractNumericScore(score);

            if (numericScore >= 80)
                return "Confiable";
            else if (numericScore >= 60)
                return "Moderado";
            else
                return "Engañoso";
        }

        private double ExtractNumericScore(string score)
        {
            try
            {
                // Remover caracteres no numéricos excepto punto decimal
                var cleanScore = score.Replace("%", "").Replace(",", ".");
                if (double.TryParse(cleanScore, out double result))
                {
                    return result;
                }
            }
            catch
            {
                // Si hay error en el parsing, devolver 0
            }
            return 0;
        }

        private string GenerateProcedure(ExamResult examResult)
        {
            var procedure = "Esta prueba le preguntará sobre los siguientes temas: ";
            var topics = new List<string>();

            if (!string.IsNullOrEmpty(examResult.ExternalExamTopic))
                topics.Add(examResult.ExternalExamTopic);

            topics.Add("Integridad personal");
            topics.Add("Comportamiento ético");
            topics.Add("Antecedentes criminales");

            for (int i = 0; i < topics.Count; i++)
            {
                procedure += $"{i + 1}. {topics[i]}";
                if (i < topics.Count - 1)
                    procedure += ". ";
            }

            procedure += ". La prueba utiliza tecnología avanzada de detección de engaño para evaluar la veracidad de las respuestas proporcionadas.";

            return procedure;
        }

        private List<string> GenerateRecommendations(ExamResult examResult)
        {
            var recommendations = new List<string>
            {
                "Obtener consentimiento por escrito antes de realizar la prueba.",
                "No confrontar directamente al examinado por un puntaje bajo.",
                "Usar los resultados como guía preventiva para la toma de decisiones.",
                "Considerar factores adicionales como referencias y antecedentes verificables.",
                "Mantener la confidencialidad de los resultados según las políticas de la empresa."
            };

            // Agregar recomendaciones específicas basadas en los resultados
            var hasLowScores = false;
            if (!string.IsNullOrEmpty(examResult.ExternalExamScore1) && ExtractNumericScore(examResult.ExternalExamScore1) < 70)
                hasLowScores = true;
            if (!string.IsNullOrEmpty(examResult.ExternalExamScore2) && ExtractNumericScore(examResult.ExternalExamScore2) < 70)
                hasLowScores = true;

            if (hasLowScores)
            {
                recommendations.Add("Se recomienda realizar entrevistas adicionales debido a los puntajes obtenidos.");
                recommendations.Add("Considerar una segunda evaluación o métodos complementarios de verificación.");
            }

            return recommendations;
        }
    }
}
