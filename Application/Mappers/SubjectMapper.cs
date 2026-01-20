using Application.DTOs.Subject;
using Domain.Entities;

public static class SubjectMapper
{
    public static Subject ToEntity(CreatedSubjectDto subjectDto, string createdBy)
    {
        return new Subject(
            subjectDto.Identification,
            subjectDto.ClientId,
            subjectDto.Name,
            subjectDto.LastName,
            subjectDto.Email,
            subjectDto.Phone,
            createdBy,
            createdBy
        )
        {
            CreatedBy = createdBy,
            ModifiedBy = createdBy
        };
    }

    public static SubjectDto ToDto(Subject subject)
    {
        return new SubjectDto
        {
            SubjectId = subject.IdentifierExternal,
            Name = subject.Name,
            LastName = subject.LastName,
            Identification = subject.Identifier,
            Email = subject.Email,
            Phone = subject.Phone
        };
    }

    public static List<SubjectDto> ToDtoList(List<Subject> subjects)
    {
        return subjects.Select(subject => ToDto(subject)).ToList();
    }
}
