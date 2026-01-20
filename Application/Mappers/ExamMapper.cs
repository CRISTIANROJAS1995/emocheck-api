using Application.DTOs.Exam;
using Domain.Entities;

public static class ExamMapper
{
    public static Exam ToEntity(CreatedExamDto examDto, string createdBy)
    {
        return new Exam(
            examDto.ExamTypeID,
            examDto.ExternalTemplateId,
            examDto.ExternalExamName,
            examDto.ExternalLocale,
            examDto.ExternalCustomerId
        )
        {
            CreatedBy = createdBy,
            ModifiedBy = createdBy,
            Company = examDto.Company,
            Description = examDto.Description!,
        };
    }

    public static Exam ToEntity(UpdateExamDto examDto, string updateBy)
    {
        var exam = new Exam(
            examDto.ExamTypeID,
            examDto.ExternalTemplateId!,
            examDto.ExternalExamName!,
            examDto.ExternalLocale!,
            examDto.ExternalCustomerId!
        )
        {
            ModifiedBy = updateBy,
            ModifiedAt = DateTime.Now,
            Company = examDto.Company!,
            Description = examDto.Description!,
        };

        return exam;
    }

    public static ExamDto ToDto(Exam exam)
    {
        return new ExamDto
        {
            ExamID = exam.ExamID,
            ExamType = new ExamTypeDto
            {
                ExamTypeID = exam.ExamType.ExamTypeID,
                Name = exam.ExamType.Name,
                Description = exam.ExamType.Description
            },
            ExternalTemplateId = exam.ExternalTemplateId,
            ExternalExamName = exam.ExternalExamName,
            ExternalLocale = exam.ExternalLocale,
            ExternalCustomerId = exam.ExternalCustomerId,
            Description = exam.Description,
            Company = exam.Company,
            CreatedBy = exam.CreatedBy,
            CreatedAt = exam.CreatedAt,
            ModifiedBy = exam.ModifiedBy,
            ModifiedAt = exam.ModifiedAt,
        };
    }

    public static List<ExamDto> ToDtoList(List<Exam> exams)
    {
        return exams.Select(exam => ToDto(exam)).ToList();
    }
}
