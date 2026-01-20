using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Exam
{
    public class CreatedExamDto
    {
        [Required(ErrorMessage = "El ExamTypeID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ExamTypeID debe ser mayor que 0.")]
        public int ExamTypeID { get; set; }

        [Required(ErrorMessage = "El ExternalTemplateId es obligatorio.")]
        public string ExternalTemplateId { get; set; }

        [Required(ErrorMessage = "El ExternalExamName es obligatorio.")]
        public string ExternalExamName { get; set; }

        [Required(ErrorMessage = "El ExternalLocale es obligatorio.")]
        public string ExternalLocale { get; set; }

        [Required(ErrorMessage = "El ExternalCustomerId es obligatorio.")]
        public string ExternalCustomerId { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "El Company es obligatorio.")]
        public string Company { get; set; }

    }
}
