namespace Domain.Entities
{
    /// <summary>
    /// Log unificado para toda la jerarquía de licencias
    /// Registra todas las operaciones: compra, asignación, uso, sobregiro, etc.
    /// </summary>
    public class LicenseLog
    {
        public int LicenseLogID { get; set; }
        
        /// <summary>
        /// Nivel de la jerarquía: 'BusinessGroup', 'Company', 'Area', 'City'
        /// </summary>
        public string LicenseLevel { get; set; }
        
        /// <summary>
        /// ID de la entidad en ese nivel
        /// </summary>
        public int EntityID { get; set; }
        
        /// <summary>
        /// Tipo de acción: 'purchase', 'assign', 'use', 'overdraft', 'refund', 'release'
        /// </summary>
        public string ActionType { get; set; }
        
        /// <summary>
        /// Cantidad de licencias en esta operación
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Balance antes de la operación
        /// </summary>
        public int BalanceBefore { get; set; }
        
        /// <summary>
        /// Balance después de la operación
        /// </summary>
        public int BalanceAfter { get; set; }
        
        /// <summary>
        /// ID de la licencia individual si aplica
        /// </summary>
        public int? LicenseID { get; set; }
        
        /// <summary>
        /// ID del ExamResult relacionado (cuando es 'use')
        /// </summary>
        public int? RelatedExamResultID { get; set; }
        
        /// <summary>
        /// Descripción adicional de la operación
        /// </summary>
        public string Description { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        private LicenseLog() { }

        public LicenseLog(
            string licenseLevel,
            int entityID,
            string actionType,
            int quantity,
            int balanceBefore,
            int balanceAfter,
            string createdBy,
            int? licenseID = null,
            int? relatedExamResultID = null,
            string description = null)
        {
            if (string.IsNullOrEmpty(licenseLevel))
            {
                throw new ArgumentException("LicenseLevel no puede ser nulo o vacío.");
            }

            if (string.IsNullOrEmpty(actionType))
            {
                throw new ArgumentException("ActionType no puede ser nulo o vacío.");
            }

            LicenseLevel = licenseLevel;
            EntityID = entityID;
            ActionType = actionType;
            Quantity = quantity;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            LicenseID = licenseID;
            RelatedExamResultID = relatedExamResultID;
            Description = description;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Crea un log de compra de licencias
        /// </summary>
        public static LicenseLog CreatePurchaseLog(int businessGroupID, int quantity, int balanceBefore, int balanceAfter, string createdBy)
        {
            return new LicenseLog(
                licenseLevel: "BusinessGroup",
                entityID: businessGroupID,
                actionType: "purchase",
                quantity: quantity,
                balanceBefore: balanceBefore,
                balanceAfter: balanceAfter,
                createdBy: createdBy,
                description: $"Compra de {quantity} licencias"
            );
        }

        /// <summary>
        /// Crea un log de asignación de licencias
        /// </summary>
        public static LicenseLog CreateAssignLog(string level, int entityID, int quantity, int balanceBefore, int balanceAfter, string createdBy, string targetName)
        {
            return new LicenseLog(
                licenseLevel: level,
                entityID: entityID,
                actionType: "assign",
                quantity: quantity,
                balanceBefore: balanceBefore,
                balanceAfter: balanceAfter,
                createdBy: createdBy,
                description: $"Asignación de {quantity} licencias a {targetName}"
            );
        }

        /// <summary>
        /// Crea un log de uso de licencia
        /// </summary>
        public static LicenseLog CreateUseLog(string level, int entityID, int licenseID, int examResultID, int balanceBefore, int balanceAfter, string createdBy)
        {
            return new LicenseLog(
                licenseLevel: level,
                entityID: entityID,
                actionType: "use",
                quantity: 1,
                balanceBefore: balanceBefore,
                balanceAfter: balanceAfter,
                createdBy: createdBy,
                licenseID: licenseID,
                relatedExamResultID: examResultID,
                description: "Licencia consumida por creación de resultado de examen"
            );
        }

        /// <summary>
        /// Crea un log de sobregiro
        /// </summary>
        public static LicenseLog CreateOverdraftLog(int businessGroupID, int balanceBefore, int balanceAfter, string createdBy)
        {
            return new LicenseLog(
                licenseLevel: "BusinessGroup",
                entityID: businessGroupID,
                actionType: "overdraft",
                quantity: 1,
                balanceBefore: balanceBefore,
                balanceAfter: balanceAfter,
                createdBy: createdBy,
                description: "Uso de licencia en sobregiro"
            );
        }

        /// <summary>
        /// Crea un log de liberación de licencias
        /// </summary>
        public static LicenseLog CreateReleaseLog(string level, int entityID, int quantity, int balanceBefore, int balanceAfter, string createdBy)
        {
            return new LicenseLog(
                licenseLevel: level,
                entityID: entityID,
                actionType: "release",
                quantity: quantity,
                balanceBefore: balanceBefore,
                balanceAfter: balanceAfter,
                createdBy: createdBy,
                description: $"Liberación de {quantity} licencias"
            );
        }
    }
}
