namespace Domain.Entities
{
    /// <summary>
    /// Licencias a nivel de Ciudad (opcional en la jerarquía)
    /// </summary>
    public class CityLicense
    {
        public int CityLicenseID { get; set; }
        public int CityID { get; set; }
        public City City { get; set; }
        public int AreaLicenseID { get; set; }
        public AreaLicense AreaLicense { get; set; }

        /// <summary>
        /// Licencias asignadas desde el área
        /// </summary>
        public int AllocatedLicenses { get; set; }

        /// <summary>
        /// Licencias asignadas a niveles inferiores (para futura expansión)
        /// </summary>
        public int AssignedLicenses { get; set; }

        /// <summary>
        /// Licencias realmente consumidas
        /// </summary>
        public int UsedLicenses { get; set; }

        /// <summary>
        /// Licencias disponibles (Calculado: AllocatedLicenses - AssignedLicenses - UsedLicenses)
        /// </summary>
        public int AvailableLicenses => AllocatedLicenses - AssignedLicenses - UsedLicenses;

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        private CityLicense() { }

        public CityLicense(int cityID, int areaLicenseID, int allocatedLicenses, string createdBy, string modifiedBy)
        {
            if (allocatedLicenses < 0)
            {
                throw new ArgumentException("AllocatedLicenses no puede ser negativo.");
            }

            CityID = cityID;
            AreaLicenseID = areaLicenseID;
            AllocatedLicenses = allocatedLicenses;
            AssignedLicenses = 0;
            UsedLicenses = 0;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        public void AllocateMore(int quantity, string modifiedBy)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            }

            AllocatedLicenses += quantity;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        public void MarkAsUsed(string modifiedBy)
        {
            if (AvailableLicenses <= 0)
            {
                throw new InvalidOperationException("No hay licencias disponibles para usar.");
            }

            UsedLicenses++;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
