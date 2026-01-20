using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CompanyLicense
{
    public class ReleaseLicenseDto
    {
        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        public int ExamResultId { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "El campo ReleasedBy no puede exceder los 100 caracteres")]
        public string ReleasedBy { get; set; } = string.Empty;
    }
}