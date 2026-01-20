using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CompanyLicense
{
    public class ConsumeLicenseDto
    {
        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        public int ExamResultId { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "El campo ConsumedBy no puede exceder los 100 caracteres")]
        public string ConsumedBy { get; set; } = string.Empty;
    }
}