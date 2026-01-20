namespace Domain.Entities
{
    /// <summary>
    /// Licencias a nivel de Empresa (Company)
    /// Recibe licencias del BusinessGroup y las distribuye a sus áreas
    /// </summary>
    public class CompanyLicense
    {
        public int CompanyLicenseID { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        
        public int? BusinessGroupLicenseID { get; set; }
        public BusinessGroupLicense BusinessGroupLicense { get; set; }
        
        /// <summary>
        /// Licencias asignadas desde el grupo empresarial
        /// </summary>
        public int AllocatedLicenses { get; set; }
        
        /// <summary>
        /// Licencias asignadas a áreas de la empresa
        /// </summary>
        public int AssignedLicenses { get; set; }
        
        /// <summary>
        /// Licencias realmente consumidas
        /// </summary>
        public int UsedLicenses { get; set; }
        
        /// <summary>
        /// Licencias disponibles para asignar (Calculado: AllocatedLicenses - AssignedLicenses)
        /// </summary>
        public int AvailableLicenses => AllocatedLicenses - AssignedLicenses;
        
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICollection<AreaLicense> AreaLicenses { get; set; } = new List<AreaLicense>();

        private CompanyLicense() { }

        public CompanyLicense(int companyID, int businessGroupLicenseID, int allocatedLicenses, string createdBy, string modifiedBy)
        {
            if (allocatedLicenses < 0)
            {
                throw new ArgumentException("AllocatedLicenses no puede ser negativo.");
            }

            CompanyID = companyID;
            BusinessGroupLicenseID = businessGroupLicenseID;
            AllocatedLicenses = allocatedLicenses;
            AssignedLicenses = 0;
            UsedLicenses = 0;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// Asigna más licencias a esta empresa (desde el grupo empresarial)
        /// </summary>
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

        /// <summary>
        /// Asigna licencias a un área
        /// </summary>
        public void AssignToArea(int quantity, string modifiedBy)
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

        /// <summary>
        /// Libera licencias de un área
        /// </summary>
        public void ReleaseFromArea(int quantity, string modifiedBy)
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

        /// <summary>
        /// Marca una licencia como usada
        /// </summary>
        public void MarkAsUsed(string modifiedBy)
        {
            UsedLicenses++;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
