using Application.DTOs.BusinessGroup;
using Application.DTOs.Country;
using Application.DTOs.State;
using Application.DTOs.CompanyLicense;

namespace Application.DTOs.Company
{
    public class CompanyDto
    {
        public int CompanyID { get; set; }
        public BusinessGroupDto BusinessGroup { get; set; }
        public CountryDto Country { get; set; }
        public StateDto State { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Nit { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsVisible { get; set; }
        public CompanyLicenseDto? Licenses { get; set; }
        public List<CompanyLicenseLogDto>? LicenseHistory { get; set; }
    }
}
