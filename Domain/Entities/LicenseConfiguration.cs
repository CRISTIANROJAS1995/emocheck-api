namespace Domain.Entities
{
    /// <summary>
    /// Configuración global del sistema de licencias
    /// Permite configurar parámetros como el máximo de licencias en sobregiro
    /// </summary>
    public class LicenseConfiguration
    {
        public int LicenseConfigurationID { get; set; }
        public string ConfigKey { get; set; }
        public int ConfigValue { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        // Propiedades de conveniencia para sobregiro
        public int MaxOverdraftLicenses => ConfigKey == "MaxOverdraftLicenses" ? ConfigValue : 0;
        public bool IsOverdraftEnabled => ConfigKey == "OverdraftEnabled" && ConfigValue == 1;

        private LicenseConfiguration() { }

        public LicenseConfiguration(string configKey, int configValue, string description, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(configKey))
            {
                throw new ArgumentException("ConfigKey no puede ser nulo o vacío.");
            }

            ConfigKey = configKey;
            ConfigValue = configValue;
            Description = description;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        public void UpdateValue(int newValue, string modifiedBy)
        {
            ConfigValue = newValue;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }

        public void UpdateMaxOverdraft(int newValue)
        {
            if (ConfigKey == "MaxOverdraftLicenses")
            {
                ConfigValue = newValue;
                ModifiedAt = DateTime.Now;
            }
        }
    }
}
