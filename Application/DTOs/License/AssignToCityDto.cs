using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.License
{
    /// <summary>
    /// DTO para asignar licencias desde un Area a una City específica
    /// </summary>
    public class AssignToCityDto
    {
        [Required(ErrorMessage = "El ID del Area es requerido")]
        public int AreaID { get; set; }

        [Required(ErrorMessage = "El ID de la Ciudad es requerido")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "La cantidad de licencias es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// DTO de respuesta después de asignar licencias a una City
    /// </summary>
    public class AssignToCityResponseDto
    {
        public int CityLicenseID { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int AreaID { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public int AllocatedLicenses { get; set; }
        public int AssignedLicenses { get; set; }
        public int UsedLicenses { get; set; }
        public int AvailableLicenses { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
