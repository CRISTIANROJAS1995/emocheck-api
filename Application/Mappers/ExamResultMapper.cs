using Application.DTOs.ExamResult;
using Domain.Entities;

public static class ExamResultMapper
{
    public static ExamResultDto ToDto(ExamResult examResult)
    {
        return new ExamResultDto
        {
            ExamResultID = examResult.ExamResultID,
            ExamSubject = ExamSubjectMapper.ToDto(examResult.ExamSubject),
            ExternalExamErrors = examResult.ExternalExamErrors,
            ExternalExamModel = examResult.ExternalExamModel,
            ExternalExamQuestions = examResult.ExternalExamQuestions,
            ExternalExamQueued = examResult.ExternalExamQueued?.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") ?? string.Empty,
            ExternalExamResult1 = examResult.ExternalExamResult1,
            ExternalExamResult2 = examResult.ExternalExamResult2,
            ExternalExamResult3 = examResult.ExternalExamResult3,
            ExternalExamScore1 = FormatScore(examResult.ExternalExamScore1),
            ExternalExamScore2 = FormatScore(examResult.ExternalExamScore2),
            ExternalExamScore3 = FormatScore(examResult.ExternalExamScore3),
            ExternalExamScore4 = FormatScore(examResult.ExternalExamScore4),
            ExternalExamScored = examResult.ExternalExamScored?.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ") ?? string.Empty,
            ExternalExamTimeouts = examResult.ExternalExamTimeouts,
            ExternalExamTopic = examResult.ExternalExamTopic,
            ExternalTemplateType = examResult.ExternalTemplateType,
            ResultExamId = examResult.ResultExamId
        };
    }

    public static List<ExamResultDto> ToDtoList(List<ExamResult> examResults)
    {
        return examResults.Select(examResult => ToDto(examResult)).ToList();
    }

    private static string FormatScore(string? score)
    {
        if (string.IsNullOrWhiteSpace(score))
            return "N/A";

        if (!decimal.TryParse(score, out decimal scoreValue))
            return score;

        // Convertir a porcentaje y formatear
        decimal percentage = scoreValue * 100;
        return $"{percentage:F2}%";
    }
}
