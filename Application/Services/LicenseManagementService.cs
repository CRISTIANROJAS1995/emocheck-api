using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class LicenseManagementService : ILicenseManagementService
    {
        private readonly IBusinessGroupLicenseRepository _businessGroupLicenseRepository;
        private readonly ICompanyLicenseRepository _companyLicenseRepository;
        private readonly IAreaLicenseRepository _areaLicenseRepository;
        private readonly ICityLicenseRepository _cityLicenseRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ILicenseLogRepository _licenseLogRepository;
        private readonly ILicenseConfigurationRepository _licenseConfigurationRepository;

        public LicenseManagementService(
            IBusinessGroupLicenseRepository businessGroupLicenseRepository,
            ICompanyLicenseRepository companyLicenseRepository,
            IAreaLicenseRepository areaLicenseRepository,
            ICityLicenseRepository cityLicenseRepository,
            ICompanyRepository companyRepository,
            IAreaRepository areaRepository,
            ICityRepository cityRepository,
            ILicenseLogRepository licenseLogRepository,
            ILicenseConfigurationRepository licenseConfigurationRepository)
        {
            _businessGroupLicenseRepository = businessGroupLicenseRepository;
            _companyLicenseRepository = companyLicenseRepository;
            _areaLicenseRepository = areaLicenseRepository;
            _cityLicenseRepository = cityLicenseRepository;
            _companyRepository = companyRepository;
            _areaRepository = areaRepository;
            _cityRepository = cityRepository;
            _licenseLogRepository = licenseLogRepository;
            _licenseConfigurationRepository = licenseConfigurationRepository;
        }

        public async Task<BusinessGroupLicense> PurchaseLicensesAsync(int businessGroupID, int quantity, string createdBy)
        {
            if (quantity <= 0)
                throw new ValidationException("La cantidad debe ser mayor a cero");

            var license = await _businessGroupLicenseRepository.GetByBusinessGroupIdAsync(businessGroupID);

            if (license == null)
            {
                license = new BusinessGroupLicense(businessGroupID, quantity, createdBy, createdBy);
                await _businessGroupLicenseRepository.AddAsync(license);
            }
            else
            {
                license.PurchasedLicenses += quantity;
                license.ModifiedBy = createdBy;
                license.ModifiedAt = DateTime.Now;
                await _businessGroupLicenseRepository.UpdateAsync(license);
            }

            // Registrar en el log
            var log = new LicenseLog(
                "BusinessGroup",
                businessGroupID,
                "Purchase",
                quantity,
                license.PurchasedLicenses - quantity,
                license.PurchasedLicenses,
                createdBy
            );
            await _licenseLogRepository.AddAsync(log);

            return license;
        }

        public async Task<BusinessGroupLicense> GetBusinessGroupLicenseStatusAsync(int businessGroupID)
        {
            var license = await _businessGroupLicenseRepository.GetByBusinessGroupIdWithDetailsAsync(businessGroupID);
            if (license == null)
                throw new EntityNotFoundException($"No se encontro licencia para el BusinessGroup {businessGroupID}");

            return license;
        }

        public async Task<CompanyLicense> AssignLicensesToCompanyAsync(int businessGroupID, int companyID, int quantity, string assignedBy)
        {
            // 1. Validar cantidad
            if (quantity <= 0)
                throw new ValidationException("La cantidad debe ser mayor a cero");

            // 2. Validar disponibilidad en BusinessGroup
            var bgLicense = await _businessGroupLicenseRepository.GetByBusinessGroupIdAsync(businessGroupID);
            if (bgLicense == null)
            {
                throw new EntityNotFoundException($"No se encontraron licencias para el BusinessGroup {businessGroupID}");
            }

            // 2.1 Validar licencias disponibles o sobregiro
            if (bgLicense.AvailableLicenses < quantity)
            {
                // Calcular cuántas licencias faltan
                int deficit = quantity - bgLicense.AvailableLicenses;

                // Verificar si el sobregiro está habilitado y hay capacidad
                var canUseOverdraft = await CanUseOverdraftAsync(businessGroupID);
                var maxOverdraft = await GetMaxOverdraftLicensesAsync();
                var currentOverdraft = bgLicense.OverdraftLicenses;

                if (!canUseOverdraft || (currentOverdraft + deficit) > maxOverdraft)
                {
                    throw new InvalidOperationException(
                        $"Licencias insuficientes en BusinessGroup. " +
                        $"Disponibles: {bgLicense.AvailableLicenses}, " +
                        $"Solicitadas: {quantity}, " +
                        $"Sobregiro actual: {currentOverdraft}/{maxOverdraft}"
                    );
                }

                // Usar sobregiro para completar la asignación
                bgLicense.UseOverdraft(deficit, assignedBy);
            }

            // 3. Validar que la empresa existe
            var company = await _companyRepository.GetByIdAsync(companyID);
            if (company == null)
            {
                throw new EntityNotFoundException($"No se encontró la empresa con ID {companyID}");
            }

            // 4. Buscar o crear CompanyLicense
            var companyLicense = await _companyLicenseRepository.GetLicenseByCompanyIdAsync(companyID);

            if (companyLicense == null)
            {
                // Crear nueva CompanyLicense
                companyLicense = new CompanyLicense(
                    companyID: companyID,
                    businessGroupLicenseID: bgLicense.BusinessGroupLicenseID,
                    allocatedLicenses: quantity,
                    createdBy: assignedBy,
                    modifiedBy: assignedBy
                );
                await _companyLicenseRepository.AddAsync(companyLicense);
            }
            else
            {
                // Agregar más licencias a la empresa existente
                companyLicense.AllocateMore(quantity, assignedBy);
                await _companyLicenseRepository.UpdateAsync(companyLicense);
            }

            // 5. Actualizar BusinessGroupLicense (incrementar AssignedLicenses)
            bgLicense.AssignToCompany(quantity, assignedBy);
            await _businessGroupLicenseRepository.UpdateAsync(bgLicense);

            // 6. Registrar en LicenseLog (2 registros: BusinessGroup y Company)
            // Log para BusinessGroup (disminución de disponibles)
            var bgLog = LicenseLog.CreateAssignLog(
                level: "BusinessGroup",
                entityID: businessGroupID,
                quantity: quantity,
                balanceBefore: bgLicense.AvailableLicenses + quantity,
                balanceAfter: bgLicense.AvailableLicenses,
                createdBy: assignedBy,
                targetName: $"Company {company.Name}"
            );
            await _licenseLogRepository.AddAsync(bgLog);

            // Log para Company (incremento de asignadas)
            var companyLog = LicenseLog.CreateAssignLog(
                level: "Company",
                entityID: companyID,
                quantity: quantity,
                balanceBefore: companyLicense.AllocatedLicenses - quantity,
                balanceAfter: companyLicense.AllocatedLicenses,
                createdBy: assignedBy,
                targetName: "From BusinessGroup"
            );
            await _licenseLogRepository.AddAsync(companyLog);

            return companyLicense;
        }

        public async Task<CompanyLicense> GetCompanyLicenseStatusAsync(int companyID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReleaseLicensesFromCompanyAsync(int companyID, int quantity, string releasedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<AreaLicense> AssignLicensesToAreaAsync(int companyID, int areaID, int quantity, string assignedBy)
        {
            // 1. Validar cantidad
            if (quantity <= 0)
                throw new ValidationException("La cantidad debe ser mayor a cero");

            // 2. Validar que la empresa tiene CompanyLicense
            var companyLicense = await _companyLicenseRepository.GetLicenseByCompanyIdAsync(companyID);
            if (companyLicense == null)
            {
                throw new EntityNotFoundException($"No se encontraron licencias para la empresa {companyID}");
            }

            // 3. Validar disponibilidad en Company
            if (companyLicense.AvailableLicenses < quantity)
            {
                throw new InvalidOperationException($"Licencias insuficientes en la empresa. Disponibles: {companyLicense.AvailableLicenses}, Solicitadas: {quantity}");
            }

            // 4. Validar que el área existe
            var area = await _areaRepository.GetByIdAsync(areaID);
            if (area == null)
            {
                throw new EntityNotFoundException($"No se encontró el área con ID {areaID}");
            }

            // 5. Buscar o crear AreaLicense
            var areaLicense = await _areaLicenseRepository.GetByAreaIdAsync(areaID);

            if (areaLicense == null)
            {
                // Crear nueva AreaLicense
                areaLicense = new AreaLicense(
                    areaID: areaID,
                    companyLicenseID: companyLicense.CompanyLicenseID,
                    allocatedLicenses: quantity,
                    createdBy: assignedBy,
                    modifiedBy: assignedBy
                );
                await _areaLicenseRepository.AddAsync(areaLicense);
            }
            else
            {
                // Agregar más licencias al área existente
                areaLicense.AllocateMore(quantity, assignedBy);
                await _areaLicenseRepository.UpdateAsync(areaLicense);
            }

            // 6. Actualizar CompanyLicense (incrementar AssignedLicenses)
            companyLicense.AssignToArea(quantity, assignedBy);
            await _companyLicenseRepository.UpdateAsync(companyLicense);

            // 7. Registrar en LicenseLog (2 registros: Company y Area)
            // Log para Company (disminución de disponibles)
            var companyLog = LicenseLog.CreateAssignLog(
                level: "Company",
                entityID: companyID,
                quantity: quantity,
                balanceBefore: companyLicense.AvailableLicenses + quantity,
                balanceAfter: companyLicense.AvailableLicenses,
                createdBy: assignedBy,
                targetName: $"Area {area.Name}"
            );
            await _licenseLogRepository.AddAsync(companyLog);

            // Log para Area (incremento de asignadas)
            var areaLog = LicenseLog.CreateAssignLog(
                level: "Area",
                entityID: areaID,
                quantity: quantity,
                balanceBefore: areaLicense.AllocatedLicenses - quantity,
                balanceAfter: areaLicense.AllocatedLicenses,
                createdBy: assignedBy,
                targetName: "From Company"
            );
            await _licenseLogRepository.AddAsync(areaLog);

            return areaLicense;
        }

        public async Task<AreaLicense> GetAreaLicenseStatusAsync(int areaID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReleaseLicensesFromAreaAsync(int areaID, int quantity, string releasedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<CityLicense> AssignLicensesToCityAsync(int areaID, int cityID, int quantity, string assignedBy)
        {
            // 1. Validar cantidad
            if (quantity <= 0)
                throw new ValidationException("La cantidad debe ser mayor a cero");

            // 2. Validar que el área tiene AreaLicense
            var areaLicense = await _areaLicenseRepository.GetByAreaIdAsync(areaID);
            if (areaLicense == null)
            {
                throw new EntityNotFoundException($"No se encontraron licencias para el área {areaID}");
            }

            // 3. Validar disponibilidad en Area
            if (areaLicense.AvailableLicenses < quantity)
            {
                throw new InvalidOperationException($"Licencias insuficientes en el área. Disponibles: {areaLicense.AvailableLicenses}, Solicitadas: {quantity}");
            }

            // 4. Validar que la ciudad existe
            var city = await _cityRepository.GetByIdAsync(cityID);
            if (city == null)
            {
                throw new EntityNotFoundException($"No se encontró la ciudad con ID {cityID}");
            }

            // 5. Buscar o crear CityLicense
            var cityLicense = await _cityLicenseRepository.GetByAreaIdAndCityIdAsync(areaID, cityID);

            if (cityLicense == null)
            {
                // Crear nueva CityLicense
                cityLicense = new CityLicense(
                    cityID: cityID,
                    areaLicenseID: areaLicense.AreaLicenseID,
                    allocatedLicenses: quantity,
                    createdBy: assignedBy,
                    modifiedBy: assignedBy
                );
                await _cityLicenseRepository.AddAsync(cityLicense);
            }
            else
            {
                // Agregar más licencias a la ciudad existente
                cityLicense.AllocateMore(quantity, assignedBy);
                await _cityLicenseRepository.UpdateAsync(cityLicense);
            }

            // 6. Actualizar AreaLicense (incrementar AssignedLicenses)
            areaLicense.AssignToCity(quantity, assignedBy);
            await _areaLicenseRepository.UpdateAsync(areaLicense);

            // 7. Registrar en LicenseLog (2 registros: Area y City)
            // Log para Area (disminución de disponibles)
            var areaLog = LicenseLog.CreateAssignLog(
                level: "Area",
                entityID: areaID,
                quantity: quantity,
                balanceBefore: areaLicense.AvailableLicenses + quantity,
                balanceAfter: areaLicense.AvailableLicenses,
                createdBy: assignedBy,
                targetName: $"City {city.Name}"
            );
            await _licenseLogRepository.AddAsync(areaLog);

            // Log para City (incremento de asignadas)
            var cityLog = LicenseLog.CreateAssignLog(
                level: "City",
                entityID: cityID,
                quantity: quantity,
                balanceBefore: cityLicense.AllocatedLicenses - quantity,
                balanceAfter: cityLicense.AllocatedLicenses,
                createdBy: assignedBy,
                targetName: "From Area"
            );
            await _licenseLogRepository.AddAsync(cityLog);

            return cityLicense;
        }

        public async Task<CityLicense> GetCityLicenseStatusAsync(int cityID, int areaID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReleaseLicensesFromCityAsync(int cityID, int areaID, int quantity, string releasedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> MarkLicenseAsUsedAsync(int examSubjectID, int examResultID, string modifiedBy)
        {
            // Este método ya no necesita la tabla License individual
            // El consumo se maneja incrementando UsedLicenses en los contadores de jerarquía
            // cuando se crea el ExamResult desde ResultService

            // Por ahora retornamos true - la lógica real se maneja desde ResultService
            // incrementando directamente los contadores según el Area/City del ExamSubject
            return await Task.FromResult(true);
        }

        public async Task<bool> CanUseOverdraftAsync(int businessGroupID)
        {
            // Obtener el límite máximo de sobregiro configurado
            var maxOverdraft = await _licenseConfigurationRepository.GetMaxOverdraftLicensesAsync();

            if (maxOverdraft <= 0)
                return false; // Sobregiro deshabilitado

            // Obtener el sobregiro actual del grupo empresarial
            var bgLicense = await _businessGroupLicenseRepository.GetByBusinessGroupIdAsync(businessGroupID);
            if (bgLicense == null)
                return false;

            // Verificar si aún hay capacidad de sobregiro disponible
            return bgLicense.OverdraftLicenses < maxOverdraft;
        }

        public async Task<int> GetMaxOverdraftLicensesAsync()
        {
            return await _licenseConfigurationRepository.GetMaxOverdraftLicensesAsync();
        }

        public async Task<bool> UpdateMaxOverdraftLicensesAsync(int newValue, string modifiedBy)
        {
            if (newValue < 0)
                throw new ValidationException("El valor de sobregiro máximo debe ser mayor o igual a cero");

            await _licenseConfigurationRepository.UpdateConfigValueAsync(
                "MaxOverdraftLicenses",
                newValue,
                modifiedBy
            );

            return true;
        }

        public async Task<List<LicenseLog>> GetLicenseHistoryAsync(string level, int entityID)
        {
            throw new NotImplementedException();
        }

        public async Task<object> GetConsolidatedLicenseReportAsync(int businessGroupID)
        {
            throw new NotImplementedException();
        }
    }
}
