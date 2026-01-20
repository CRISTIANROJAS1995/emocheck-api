using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.ExamSubject
{
    public class CreatedExamSubjectDto
    {
        [Required(ErrorMessage = "El SubjectId es obligatorio.")]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = "El TemplateId es obligatorio.")]
        public string TemplateId { get; set; }
    }
}
