using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CompanyLicense
{
    public class LicenseActionDto
    {
        [Required]
        public string Action { get; set; } = string.Empty; // "assign", "consume"
        
        public int? TotalLicenses { get; set; }
        
        public int? ExamResultId { get; set; }
    }
}