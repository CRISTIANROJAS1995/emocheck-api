using Application.DTOs.Exam;
using Application.DTOs.ExamSubject;
using Domain.Entities;

public static class ExamSubjectMapper
{
    public static ExamSubjectDto ToDto(ExamSubject examSubject)
    {
        return new ExamSubjectDto
        {
            ExamSubjectID = examSubject.ExamSubjectID,
            Exam = ExamMapper.ToDto(examSubject.Exam),
            Subject = SubjectMapper.ToDto(examSubject.Subject),
            ExternalExamId = examSubject.ExternalExamId,
            ExternalExamUrl = examSubject.ExternalExamUrl,
            ExternalExamQueued = examSubject.ExternalExamQueued,
            ExternalExamStatus = MapLinkStatus(examSubject.ExternalExamStatus),
            ExternalExamStep = examSubject.ExternalExamStep,
            CreatedBy = examSubject.CreatedBy,
            CreatedAt = examSubject.CreatedAt,
            ModifiedBy = examSubject.ModifiedBy,
            ModifiedAt = examSubject.ModifiedAt,
        };
    }

    public static List<ExamSubjectDto> ToDtoList(List<ExamSubject> examSubjects)
    {
        return examSubjects.Select(examSubject => ToDto(examSubject)).ToList();
    }

    private static string MapLinkStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return "Link creado (estado inicial)";

        if (!int.TryParse(status, out int code))
            return $"Estado inválido ({status})";

        return code switch
        {
            2 => "Link clickeado",
            3 => "Inicio de prueba real",
            4 => "Prueba completada - procesando",
            5 => "Prueba procesada - subiendo",
            6 => "Prueba subida",
            7 => "Procesando audio",
            8 => "Procesando video",
            10 => "Esperando para calificar",
            _ => $"Estado desconocido ({code})"
        };
    }


}
