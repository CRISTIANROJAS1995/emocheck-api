using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.License
{
    public class AssignToCompanyDto
    {
        [Required(ErrorMessage = "BusinessGroupID es requerido")]
        public int BusinessGroupID { get; set; }

        [Required(ErrorMessage = "CompanyID es requerido")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "Quantity es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }

    public class AssignToCompanyResponseDto
    {
        public int CompanyLicenseID { get; set; }
        public int CompanyID { get; set; }
        public int AllocatedLicenses { get; set; }
        public int AssignedLicenses { get; set; }
        public int UsedLicenses { get; set; }
        public int AvailableLicenses { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
