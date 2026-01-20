using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.License
{
    public class AssignToAreaDto
    {
        [Required(ErrorMessage = "CompanyID es requerido")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "AreaID es requerido")]
        public int AreaID { get; set; }

        [Required(ErrorMessage = "Quantity es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }

    public class AssignToAreaResponseDto
    {
        public int AreaLicenseID { get; set; }
        public int AreaID { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public int AllocatedLicenses { get; set; }
        public int AssignedLicenses { get; set; }
        public int UsedLicenses { get; set; }
        public int AvailableLicenses { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
