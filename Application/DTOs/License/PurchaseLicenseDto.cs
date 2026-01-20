namespace Application.DTOs.License
{
    public class PurchaseLicenseDto
    {
        public int BusinessGroupID { get; set; }
        public int Quantity { get; set; }
        public string PurchasedBy { get; set; } = string.Empty;
    }

    public class PurchaseLicenseResponseDto
    {
        public int BusinessGroupLicenseID { get; set; }
        public int PurchasedQuantity { get; set; }
        public int TotalPurchasedLicenses { get; set; }
        public int AvailableLicenses { get; set; }
        public int OverdraftLicenses { get; set; }
    }
}
