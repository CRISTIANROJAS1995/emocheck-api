using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CompanyLicense
{
    public class AssignLicensesDto
    {
        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El número de licencias debe ser mayor a 0")]
        public int TotalLicenses { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "El campo AssignedBy no puede exceder los 100 caracteres")]
        public string AssignedBy { get; set; } = string.Empty;
    }
}