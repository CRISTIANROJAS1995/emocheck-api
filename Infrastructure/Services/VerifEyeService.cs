using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs.VerifEye;
using Domain.Extension.Reports;
using Domain.Extension.VerifEye;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class VerifEyeService : IVerifEyeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VerifEyeService> _logger;

        public VerifEyeService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<VerifEyeService> logger)
        {
            _logger = logger;

            var base64 = configuration["VerifEye:AuthBase64"];
            if (string.IsNullOrEmpty(base64))
                throw new ArgumentException("VerifEye AuthBase64 is not configured.");

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://secure.converus.net/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);
        }

        public async Task<string> CreateExamineeAsync(string name, string email, string mobile, string token)
        {
            var payload = new
            {
                subjectName = name,
                subjectEmail = email,
                subjectMobile = mobile,
                subjectToken = token
            };

            var response = await _httpClient.PostAsJsonAsync("api/VerifEye/subject", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CreateExamineeResponse>();
            return result?.SubjectId ?? throw new InvalidOperationException("No SubjectId returned.");
        }

        public async Task<List<VerifEyeTestTemplate>> ListAvailableTestsAsync()
        {
            var response = await _httpClient.GetAsync("api/VerifEye/catalog");
            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<List<ListTestTemplateResponse>>();
            if (items == null) return new();

            return items.Select(t => new VerifEyeTestTemplate
            {
                CustomerId = t.CustomerId,
                TemplateId = t.TemplateId,
                Name = t.ExamName,
                Locale = t.ExamLocale
            }).ToList();
        }

        public async Task<VerifEyePreparedTest> PrepareTestAsync(
            string subjectId,
            string templateId,
            Dictionary<string, string>? templateInput)
        {
            var payload = new
            {
                subjectId,
                templateId,
                templateInput = templateInput ?? new Dictionary<string, string>()
            };

            var response = await _httpClient.PostAsJsonAsync("api/VerifEye/template/input", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PrepareTestResponse>();
            if (result == null)
                throw new InvalidOperationException("Failed to prepare test.");

            return new VerifEyePreparedTest
            {
                ExamId = result.ExamId,
                ExamUrl = result.ExamUrl
            };
        }

        public async Task<List<VerifEyeQueuedExam>> ListQueuedExamsAsync()
        {
            var response = await _httpClient.GetAsync("api/VerifEye/queue");
            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<List<VerifEyeQueuedExam>>();
            return items ?? new List<VerifEyeQueuedExam>();
        }

        public async Task<VerifEyeQueuedExam?> GetQueuedExamByIdAsync(string examId)
        {
            var response = await _httpClient.GetAsync($"api/VerifEye/queue/{examId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("VerifEye exam with ID {ExamId} not found. Status code: {StatusCode}", examId, response.StatusCode);
                return null;
            }

            var rawJson = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Respuesta de Converus para el examId {ExamId}: {RawJson}", examId, rawJson);

            if (string.IsNullOrWhiteSpace(rawJson) || rawJson.Trim() == "{}" || rawJson.Contains(@"""examQueued"":"""""))
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<VerifEyeQueuedExam>();

            if (result != null &&
                string.IsNullOrWhiteSpace(result.CustomerId) &&
                string.IsNullOrWhiteSpace(result.ExamId) &&
                string.IsNullOrWhiteSpace(result.ExamLocale) &&
                result.ExamQueued == default &&
                string.IsNullOrWhiteSpace(result.ExamStatus) &&
                string.IsNullOrWhiteSpace(result.ExamStep) &&
                string.IsNullOrWhiteSpace(result.ExamUrl) &&
                string.IsNullOrWhiteSpace(result.SubjectId) &&
                string.IsNullOrWhiteSpace(result.TemplateId))
            {
                return null;
            }

            return result;
        }

        public async Task<List<VerifEyeExaminee>> ListExamineesAsync()
        {
            var response = await _httpClient.GetAsync("api/VerifEye/subject");
            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<List<ListExamineeResponse>>();
            if (items == null) return new();

            return items.Select(e => new VerifEyeExaminee
            {
                SubjectId = e.SubjectId,
                Name = e.SubjectName,
                Email = e.SubjectEmail,
                Mobile = e.SubjectMobile,
                Token = e.SubjectToken,
                Photo = e.SubjectPhoto,
                CustomerId = e.CustomerId
            }).ToList();
        }

        public async Task<VerifEyeExaminee?> GetExamineeByIdAsync(string subjectId)
        {
            var response = await _httpClient.GetAsync($"api/VerifEye/subject/{subjectId}");
            response.EnsureSuccessStatusCode();

            var item = await response.Content.ReadFromJsonAsync<ListExamineeResponse>();
            if (item == null) return null;

            return new VerifEyeExaminee
            {
                SubjectId = item.SubjectId,
                Name = item.SubjectName,
                Email = item.SubjectEmail,
                Mobile = item.SubjectMobile,
                Token = item.SubjectToken,
                Photo = item.SubjectPhoto,
                CustomerId = item.CustomerId
            };
        }

        public async Task<bool> DeleteQueuedExamAsync(string examId)
        {
            var response = await _httpClient.DeleteAsync($"api/VerifEye/queue/{examId}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Examen {ExamId} eliminado correctamente en Converus.", examId);
                return true;
            }
            else
            {
                _logger.LogWarning("No se pudo eliminar el examen {ExamId} en Converus. Status code: {StatusCode}", examId, response.StatusCode);
                return false;
            }
        }

        public async Task<List<VerifEyeExamResult>> ListExamResultsAsync()
        {
            var response = await _httpClient.GetAsync("api/VerifEye/repository");
            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<List<VerifEyeExamResult>>();
            return items ?? new List<VerifEyeExamResult>();
        }

        public async Task<ConverusExamReportData?> GetExamRepositoryByIdAsync(string examId)
        {
            var response = await _httpClient.GetAsync($"api/VerifEye/repository/{examId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No se encontró el examen en repository con id {ExamId}. Status code: {StatusCode}", examId, response.StatusCode);
                return null;
            }
            var rawJson = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Respuesta de Converus repository para el examId {ExamId}: {RawJson}", examId, rawJson);
            if (string.IsNullOrWhiteSpace(rawJson) || rawJson.Trim() == "{}")
                return null;
            return await response.Content.ReadFromJsonAsync<ConverusExamReportData>();
        }

        public async Task<List<QuestionDto>> GetExamQuestionsAsync(string templateId, string examId)
        {
            var response = await _httpClient.GetAsync($"api/VerifEye/exam/questions/{templateId}/{examId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No se encontraron preguntas para el templateId {TemplateId} y examId {ExamId}. Status code: {StatusCode}", templateId, examId, response.StatusCode);
                return new List<QuestionDto>();
            }
            var rawJson = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Respuesta de Converus questions para el templateId {TemplateId} y examId {ExamId}: {RawJson}", templateId, examId, rawJson);
            if (string.IsNullOrWhiteSpace(rawJson) || rawJson.Trim() == "[]")
                return new List<QuestionDto>();
            return await response.Content.ReadFromJsonAsync<List<QuestionDto>>() ?? new List<QuestionDto>();
        }

        public async Task<List<AnswerDto>> GetExamAnswersAsync(string examId)
        {
            var response = await _httpClient.GetAsync($"api/VerifEye/exam/answers/{examId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No se encontraron respuestas para el examId {ExamId}. Status code: {StatusCode}", examId, response.StatusCode);
                return new List<AnswerDto>();
            }
            var rawJson = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Respuesta de Converus answers para el examId {ExamId}: {RawJson}", examId, rawJson);
            if (string.IsNullOrWhiteSpace(rawJson) || rawJson.Trim() == "[]")
                return new List<AnswerDto>();

            var examAnswersResponse = JsonSerializer.Deserialize<ExamAnswersResponseDto>(rawJson);
            return examAnswersResponse?.examAnswers ?? new List<AnswerDto>();
        }
    }
}
