namespace Domain.Entities
{
    /// <summary>
    /// Licencias a nivel de Área
    /// </summary>
    public class AreaLicense
    {
        public int AreaLicenseID { get; set; }
        public int AreaID { get; set; }
        public Area Area { get; set; }
        public int CompanyLicenseID { get; set; }
        public CompanyLicense CompanyLicense { get; set; }

        /// <summary>
        /// Licencias asignadas desde la empresa
        /// </summary>
        public int AllocatedLicenses { get; set; }

        /// <summary>
        /// Licencias asignadas a ciudades
        /// </summary>
        public int AssignedLicenses { get; set; }

        /// <summary>
        /// Licencias realmente consumidas
        /// </summary>
        public int UsedLicenses { get; set; }

        /// <summary>
        /// Licencias disponibles (Calculado: AllocatedLicenses - AssignedLicenses)
        /// </summary>
        public int AvailableLicenses => AllocatedLicenses - AssignedLicenses;

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICollection<CityLicense> CityLicenses { get; set; } = new List<CityLicense>();

        private AreaLicense() { }

        public AreaLicense(int areaID, int companyLicenseID, int allocatedLicenses, string createdBy, string modifiedBy)
        {
            if (allocatedLicenses < 0)
            {
                throw new ArgumentException("AllocatedLicenses no puede ser negativo.");
            }

            AreaID = areaID;
            CompanyLicenseID = companyLicenseID;
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

        public void AssignToCity(int quantity, string modifiedBy)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            }

            if (AvailableLicenses < quantity)
            {
                throw new InvalidOperationException($"Licencias insuficientes. Disponibles: {AvailableLicenses}, Solicitadas: {quantity}");
            }

            AssignedLicenses += quantity;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        public void ReleaseFromCity(int quantity, string modifiedBy)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            }

            if (AssignedLicenses < quantity)
            {
                throw new InvalidOperationException($"No se pueden liberar {quantity} licencias. Solo hay {AssignedLicenses} asignadas.");
            }

            AssignedLicenses -= quantity;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        public void MarkAsUsed(string modifiedBy)
        {
            UsedLicenses++;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
