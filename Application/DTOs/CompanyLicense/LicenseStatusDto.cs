namespace Application.DTOs.CompanyLicense
{
    public class LicenseStatusDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int AllocatedLicenses { get; set; }
        public int UsedLicenses { get; set; }
        public int AvailableLicenses { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }
}