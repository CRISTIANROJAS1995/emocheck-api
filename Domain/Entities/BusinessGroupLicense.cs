namespace Domain.Entities
{
    /// <summary>
    /// Licencias a nivel de Grupo Empresarial (nivel superior de la jerarquía)
    /// </summary>
    public class BusinessGroupLicense
    {
        public int BusinessGroupLicenseID { get; set; }
        public int BusinessGroupID { get; set; }
        public BusinessGroup BusinessGroup { get; set; }

        /// <summary>
        /// Licencias compradas por el grupo empresarial
        /// </summary>
        public int PurchasedLicenses { get; set; }

        /// <summary>
        /// Licencias asignadas a empresas del grupo
        /// </summary>
        public int AssignedLicenses { get; set; }

        /// <summary>
        /// Licencias realmente consumidas (cuando se crea ExamResult)
        /// </summary>
        public int UsedLicenses { get; set; }

        /// <summary>
        /// Licencias en sobregiro (saldo negativo permitido)
        /// </summary>
        public int OverdraftLicenses { get; set; }

        /// <summary>
        /// Licencias disponibles para asignar (Calculado: PurchasedLicenses - AssignedLicenses)
        /// </summary>
        public int AvailableLicenses => PurchasedLicenses - AssignedLicenses;

        /// <summary>
        /// Balance real de licencias (considera sobregiro)
        /// </summary>
        public int RealBalance => PurchasedLicenses - UsedLicenses - OverdraftLicenses;

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        private BusinessGroupLicense() { }

        public BusinessGroupLicense(int businessGroupID, int purchasedLicenses, string createdBy, string modifiedBy)
        {
            if (purchasedLicenses < 0)
            {
                throw new ArgumentException("PurchasedLicenses no puede ser negativo.");
            }

            BusinessGroupID = businessGroupID;
            PurchasedLicenses = purchasedLicenses;
            AssignedLicenses = 0;
            UsedLicenses = 0;
            OverdraftLicenses = 0;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// Compra más licencias (suma al total de compradas)
        /// </summary>
        public void PurchaseMore(int quantity, string modifiedBy)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            }

            // Si hay sobregiro, descontar primero de las nuevas licencias
            if (OverdraftLicenses > 0)
            {
                int overdraftToSettle = Math.Min(quantity, OverdraftLicenses);
                OverdraftLicenses -= overdraftToSettle;
                quantity -= overdraftToSettle;
            }

            PurchasedLicenses += quantity;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// Asigna licencias a una empresa (incrementa AssignedLicenses)
        /// </summary>
        public void AssignToCompany(int quantity, string modifiedBy)
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
        /// Libera licencias asignadas (decrementa AssignedLicenses)
        /// </summary>
        public void ReleaseFromCompany(int quantity, string modifiedBy)
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
        /// Marca una licencia como usada (incrementa UsedLicenses)
        /// </summary>
        public void MarkAsUsed(string modifiedBy)
        {
            UsedLicenses++;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// Usa una licencia en sobregiro (cuando se agotan las compradas)
        /// </summary>
        public bool TryUseOverdraft(int maxOverdraftAllowed, string modifiedBy)
        {
            if (OverdraftLicenses >= maxOverdraftAllowed)
            {
                return false; // Ya se alcanzó el límite de sobregiro
            }

            OverdraftLicenses++;
            UsedLicenses++;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Usa una cantidad específica de sobregiro al asignar licencias
        /// </summary>
        public void UseOverdraft(int quantity, string modifiedBy)
        {
            OverdraftLicenses += quantity;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
