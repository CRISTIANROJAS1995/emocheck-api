using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ILicenseManagementService
    {
        // ===== BUSINESS GROUP LEVEL =====
        /// <summary>
        /// Compra licencias para un grupo empresarial
        /// </summary>
        Task<BusinessGroupLicense> PurchaseLicensesAsync(int businessGroupID, int quantity, string createdBy);

        /// <summary>
        /// Obtiene el estado de licencias del grupo empresarial
        /// </summary>
        Task<BusinessGroupLicense> GetBusinessGroupLicenseStatusAsync(int businessGroupID);

        // ===== COMPANY LEVEL =====
        /// <summary>
        /// Asigna licencias desde BusinessGroup a una Company
        /// </summary>
        Task<CompanyLicense> AssignLicensesToCompanyAsync(int businessGroupID, int companyID, int quantity, string assignedBy);

        /// <summary>
        /// Obtiene el estado de licencias de una empresa
        /// </summary>
        Task<CompanyLicense> GetCompanyLicenseStatusAsync(int companyID);

        /// <summary>
        /// Libera licencias de una empresa (devuelve al BusinessGroup)
        /// </summary>
        Task<bool> ReleaseLicensesFromCompanyAsync(int companyID, int quantity, string releasedBy);

        // ===== AREA LEVEL =====
        /// <summary>
        /// Asigna licencias desde Company a un Area
        /// </summary>
        Task<AreaLicense> AssignLicensesToAreaAsync(int companyID, int areaID, int quantity, string assignedBy);

        /// <summary>
        /// Obtiene el estado de licencias de un área
        /// </summary>
        Task<AreaLicense> GetAreaLicenseStatusAsync(int areaID);

        /// <summary>
        /// Libera licencias de un área (devuelve a la Company)
        /// </summary>
        Task<bool> ReleaseLicensesFromAreaAsync(int areaID, int quantity, string releasedBy);

        // ===== CITY LEVEL (OPCIONAL) =====
        /// <summary>
        /// Asigna licencias desde Area a una City
        /// </summary>
        Task<CityLicense> AssignLicensesToCityAsync(int areaID, int cityID, int quantity, string assignedBy);

        /// <summary>
        /// Obtiene el estado de licencias de una ciudad
        /// </summary>
        Task<CityLicense> GetCityLicenseStatusAsync(int cityID, int areaID);

        /// <summary>
        /// Libera licencias de una ciudad (devuelve al Area)
        /// </summary>
        Task<bool> ReleaseLicensesFromCityAsync(int cityID, int areaID, int quantity, string releasedBy);


        /// <summary>
        /// Marca una licencia como usada cuando se crea el resultado del examen
        /// </summary>
        Task<bool> MarkLicenseAsUsedAsync(int examSubjectID, int examResultID, string modifiedBy);

        // ===== OVERDRAFT =====
        /// <summary>
        /// Verifica si se puede usar una licencia en sobregiro
        /// </summary>
        Task<bool> CanUseOverdraftAsync(int businessGroupID);


        // ===== CONFIGURATION =====
        /// <summary>
        /// Obtiene la configuración máxima de sobregiro permitido
        /// </summary>
        Task<int> GetMaxOverdraftLicensesAsync();

        /// <summary>
        /// Actualiza la configuración máxima de sobregiro
        /// </summary>
        Task<bool> UpdateMaxOverdraftLicensesAsync(int newValue, string modifiedBy);

        // ===== REPORTS =====
        /// <summary>
        /// Obtiene el historial de logs de una entidad
        /// </summary>
        Task<List<LicenseLog>> GetLicenseHistoryAsync(string level, int entityID);

        /// <summary>
        /// Obtiene un resumen consolidado de licencias por grupo empresarial
        /// </summary>
        Task<object> GetConsolidatedLicenseReportAsync(int businessGroupID);
    }
}
