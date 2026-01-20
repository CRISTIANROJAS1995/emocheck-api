using Application.DTOs.Company;

namespace Application.DTOs.CompanyLicense
{
    public class CompanyLicenseDto
    {
        public int CompanyLicenseID { get; set; }
        public int CompanyID { get; set; }
        public CompanyDto Company { get; set; }
        public int AllocatedLicenses { get; set; }
        public int UsedLicenses { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
