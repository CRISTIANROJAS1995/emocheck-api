namespace Application.DTOs.CompanyLicense
{
    public class CompanyLicenseLogDto
    {
        //public int CompanyLicenseLogID { get; set; }
        //public int CompanyLicenseID { get; set; }
        //public CompanyLicenseDto CompanyLicense { get; set; }
        public string ActionType { get; set; }
        public int Quantity { get; set; }
        public int RelatedExamResultID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
