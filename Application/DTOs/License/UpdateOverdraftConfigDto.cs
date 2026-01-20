using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.License
{
    public class UpdateOverdraftConfigDto
    {
        [Required(ErrorMessage = "MaxOverdraftLicenses es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "MaxOverdraftLicenses debe ser mayor o igual a 0")]
        public int MaxOverdraftLicenses { get; set; }
    }
}
