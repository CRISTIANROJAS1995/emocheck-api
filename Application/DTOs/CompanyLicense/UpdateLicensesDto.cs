using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CompanyLicense
{
    public class UpdateLicensesDto
    {
        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El número de licencias no puede ser negativo")]
        public int NewTotalLicenses { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "El campo ModifiedBy no puede exceder los 100 caracteres")]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}