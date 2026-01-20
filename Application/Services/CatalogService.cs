using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Domain.Enums;
using Domain.Extension.VerifEye;

namespace Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IExamService _examService;
        private readonly IExamSubjectService _examSubjectService;
        private readonly ISubjectService _subjectService;
        private readonly IVerifEyeService _verifEyeService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(IExamService examService, IExamSubjectService examSubjectService, ISubjectService subjectService, IVerifEyeService verifEyeService, ICompanyRepository companyRepository, ILogger<CatalogService> logger)
        {
            _examService = examService;
            _examSubjectService = examSubjectService;
            _subjectService = subjectService;
            _verifEyeService = verifEyeService;
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<List<Exam>> GetAllCatalogAsync(int companyId)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(companyId);
            if (existingCompany == null)
            {
                throw new ArgumentException("El cliente enviado no existe");
            }

            var resultConverus = await _verifEyeService.ListAvailableTestsAsync();
            var existingExams = await _examService.GetAllAsync();

            // Variables para capturar los tipos de examen de Converus
            var allTestTypes = new HashSet<string>();
            var unmappedTestTypes = new HashSet<string>();

            foreach (var item in resultConverus)
            {
                var parts = item.Name.Split('-').Select(p => p.Trim()).ToList();
                if (parts.Count < 2) continue; // No tiene estructura válida

                var companyName = parts[0];
                var testTypeName = parts[1];
                var description = string.Join(" - ", parts.Skip(2));
                var testTypeKey = NormalizeText(testTypeName);

                //sino existe la compañía crearla para que los datos esten homologados
                var consultCompany = await _companyRepository.GetByNameAsync(companyName);
                if (consultCompany == null)
                {
                    Guid nit = Guid.NewGuid();
                    var company = new Company(
                        businessGroupID: 1, //Grupo empresarial inicial
                        countryID: 1, //País inicial Colombia
                        stateID: (int)StateEnum.Active,
                        name: companyName,
                        nit: nit.ToString(),
                        createdBy: "system",
                        modifiedBy: "system"
                    )
                    {
                        Description = string.Empty,
                        Address = string.Empty,
                        Phone = string.Empty,
                        IsVisible = true
                    };

                    await _companyRepository.AddAsync(company);
                }

                // Registrar todos los tipos encontrados
                allTestTypes.Add(testTypeName);

                if (!ExamTypeMap.TryGetValue(testTypeKey, out var examTypeId))
                {
                    examTypeId = 0;
                    unmappedTestTypes.Add(testTypeName);
                    _logger.LogWarning($"Tipo de prueba no reconocido: '{testTypeName}' en examen '{item.Name}'");
                    continue;
                }

                var examExistente = existingExams.FirstOrDefault(e => e.ExternalTemplateId == item.TemplateId);

                if (examExistente == null)
                {
                    // Crear nuevo examen
                    var exam = new Exam(
                        examTypeID: examTypeId,
                        externalTemplateId: item.TemplateId,
                        externalExamName: item.Name,
                        externalLocale: item.Locale,
                        externalCustomerId: item.CustomerId
                    )
                    {
                        Company = companyName,
                        CreatedBy = "system",
                        ModifiedBy = "system",
                        Description = description
                    };

                    await _examService.CreateAsync(exam);
                }
                else
                {
                    // Actualizar si hay diferencias
                    bool requiereActualizacion =
                        examExistente.ExternalExamName != item.Name ||
                        examExistente.ExternalLocale != item.Locale ||
                        examExistente.ExternalCustomerId != item.CustomerId ||
                        examExistente.Description != description ||
                        examExistente.ExamTypeID != examTypeId ||
                        examExistente.Company != companyName;

                    if (requiereActualizacion)
                    {
                        examExistente.ExternalExamName = item.Name;
                        examExistente.ExternalLocale = item.Locale;
                        examExistente.ExternalCustomerId = item.CustomerId;
                        examExistente.Description = description;
                        examExistente.ExamTypeID = examTypeId;
                        examExistente.Company = companyName;
                        examExistente.ModifiedBy = "system";
                        examExistente.ModifiedAt = DateTime.UtcNow;

                        await _examService.UpdateAsync(examExistente);
                    }
                }
            }

            // Log de resumen de tipos encontrados
            Console.WriteLine($"Tipos encontrados: {string.Join(", ", allTestTypes.OrderBy(x => x))}");
            if (unmappedTestTypes.Any())
            {
                Console.WriteLine($"Tipos NO mapeados: {string.Join(", ", unmappedTestTypes.OrderBy(x => x))}");
            }

            return await _examService.GetAllByCompanyIdAsync(existingCompany.Name);
        }

        public async Task<ExamSubject?> AssignCatalogAsync(string subjectId, string templateId, string userCreatedBy)
        {
            var existingSubject = await _subjectService.GetByIdExternalAsync(subjectId);
            if (existingSubject == null)
            {
                throw new ArgumentException("El candidato enviado no existe");
            }

            var existingCatalog = await _examService.GetByTemplateIdAsync(templateId);
            if (existingCatalog == null)
            {
                throw new ArgumentException("La prueba enviada no existe");
            }

            // Preparar el examen con Converus
            var preparedTest = await _verifEyeService.PrepareTestAsync(subjectId, templateId, null);

            // Obtener los datos detallados del examen en cola
            var queuedExam = await _verifEyeService.GetQueuedExamByIdAsync(preparedTest.ExamId);
            if (queuedExam == null)
            {
                throw new InvalidOperationException("No se pudo obtener la información del examen asignado desde la cola.");
            }

            // Buscar si ya existe el registro
            var existingExamSubject = await _examSubjectService.GetByExternalExamIdAsync(queuedExam.ExamId);

            if (existingExamSubject == null)
            {
                // Crear nuevo registro
                var examSubject = new ExamSubject(
                    examID: existingCatalog.ExamID,
                    subjectID: existingSubject.SubjectID,
                    externalExamId: queuedExam.ExamId,
                    externalExamUrl: queuedExam.ExamUrl,
                    createdBy: userCreatedBy.ToString(),
                    modifiedBy: userCreatedBy.ToString()
                )
                {
                    ExternalExamQueued = queuedExam.ExamQueued.ToString("O"),
                    ExternalExamStatus = queuedExam.ExamStatus,
                    ExternalExamStep = queuedExam.ExamStep
                };

                await _examSubjectService.CreateAsync(examSubject);
                return await _examSubjectService.GetByIdAsync(examSubject.ExamSubjectID);
            }
            else
            {
                // Actualizar si hay diferencias
                bool requiereActualizacion =
                    existingExamSubject.ExamID != existingCatalog.ExamID ||
                    existingExamSubject.SubjectID != existingSubject.SubjectID ||
                    existingExamSubject.ExternalExamUrl != queuedExam.ExamUrl ||
                    existingExamSubject.ExternalExamQueued != queuedExam.ExamQueued.ToString("O") ||
                    existingExamSubject.ExternalExamStatus != queuedExam.ExamStatus ||
                    existingExamSubject.ExternalExamStep != queuedExam.ExamStep;

                if (requiereActualizacion)
                {
                    existingExamSubject.ExamID = existingCatalog.ExamID;
                    existingExamSubject.SubjectID = existingSubject.SubjectID;
                    existingExamSubject.ExternalExamUrl = queuedExam.ExamUrl;
                    existingExamSubject.ExternalExamQueued = queuedExam.ExamQueued.ToString("O");
                    existingExamSubject.ExternalExamStatus = queuedExam.ExamStatus;
                    existingExamSubject.ExternalExamStep = queuedExam.ExamStep;
                    existingExamSubject.ModifiedBy = userCreatedBy.ToString();
                    existingExamSubject.ModifiedAt = DateTime.UtcNow;

                    await _examSubjectService.UpdateAsync(existingExamSubject);
                }

                return existingExamSubject;
            }
        }

        public async Task<List<ExamSubject>> MigrationAssignCatalogAsync()
        {
            var queues = await _verifEyeService.ListQueuedExamsAsync();
            if (queues.Count > 0)
            {
                foreach (VerifEyeQueuedExam item in queues)
                {
                    var existingSubject = await _subjectService.GetByIdExternalAsync(item.SubjectId);
                    if (existingSubject == null)
                    {
                        continue;
                    }

                    var existingCatalog = await _examService.GetByTemplateIdAsync(item.TemplateId);
                    if (existingCatalog == null)
                    {
                        continue;
                    }

                    var existingExamSubject = await _examSubjectService.GetByExternalExamIdAsync(item.ExamId);
                    if (existingExamSubject == null)
                    {
                        var examSubject = new ExamSubject(
                            examID: existingCatalog.ExamID,
                            subjectID: existingSubject.SubjectID,
                            externalExamId: item.ExamId,
                            externalExamUrl: item.ExamUrl,
                            createdBy: "system",
                            modifiedBy:"system"
                        )
                        {
                            ExternalExamQueued = item.ExamQueued.ToString("O"),
                            ExternalExamStatus = item.ExamStatus,
                            ExternalExamStep = item.ExamStep
                        };

                        await _examSubjectService.CreateAsync(examSubject);
                    }
                }
            }

            return await _examSubjectService.GetAllAsync();
        }

        private static readonly Dictionary<string, int> ExamTypeMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "preempleo", 1 },
            { "rutina", 2 },
            { "investigacion", 3 },
            { "drogas", 4 },
            { "infidelidad", 5 },
            { "robo", 7 },
            { "credito", 8 },
            { "visitadomiciliaria", 9 },
            { "r27ago", 10 },
            { "pruebarutina", 11 },
            { "fugadeinformacionconfidencial", 12 },
            { "formulario", 13 },
            { "consumodedrogas", 14 }
        };

        private static string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            var normalized = input.Trim().ToLowerInvariant();

            // Quitar tildes (áéíóú => aeiou)
            normalized = normalized
                .Normalize(System.Text.NormalizationForm.FormD)
                .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                .Aggregate("", (s, c) => s + c);

            // Quitar espacios
            normalized = normalized.Replace(" ", "");

            return normalized;
        }
    }
}
