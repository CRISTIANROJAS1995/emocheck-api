using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyLicenseRepository _companyLicenseRepository;
        private readonly IVerifEyeService _verifEyeService;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(
            ICompanyRepository companyRepository,
            ICompanyLicenseRepository companyLicenseRepository,
            IVerifEyeService verifEyeService,
            ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _companyLicenseRepository = companyLicenseRepository;
            _logger = logger;
            _verifEyeService = verifEyeService;
        }

        public async Task<List<Company>> MigrationOfConverusAsync()
        {
            var resultConverus = await _verifEyeService.ListAvailableTestsAsync();

            foreach (var item in resultConverus)
            {
                var parts = item.Name.Split('-').Select(p => p.Trim()).ToList();
                if (parts.Count < 2) continue; // No tiene estructura válida

                var companyName = parts[0];
                var consultCompany = await _companyRepository.GetByNameAsync(companyName);

                if (consultCompany == null)
                {
                    Guid nit = Guid.NewGuid();
                    var company = new Company(
                        businessGroupID: 1, //Grupo empresarial inicial
                        countryID: 1, //País inicial Colombia
                        stateID: (int)StateEnum.Active,
                        name: companyName,
                        nit: nit.ToString(),
                        createdBy: "system",
                        modifiedBy: "system"
                    )
                    {
                        Description = string.Empty,
                        Address = string.Empty,
                        Phone = string.Empty,
                        IsVisible = true
                    };

                    await _companyRepository.AddAsync(company);
                }
            }

            return await _companyRepository.GetAllCompanyAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            return await _companyRepository.GetCompanyByIdAsync(companyId);
        }

        public async Task<List<Company>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.ToList();
        }

        public async Task<List<Company>> GetByUserIdAsync(int userId)
        {
            return await _companyRepository.GetByUserIdAsync(userId);
        }

        public async Task<Company> CreateAsync(Company company)
        {
            var existing = await _companyRepository.GetByNameAsync(company.Name);
            if (existing != null)
            {
                throw new EntityAlreadyExistsException("La compañía");
            }

            try
            {
                await _companyRepository.AddAsync(company);
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la compañía: {ex.Message}");
                throw new InvalidOperationException("Error al crear la compañía", ex);
            }
        }

        public async Task UpdateAsync(Company company)
        {
            var existing = await _companyRepository.GetByIdAsync(company.CompanyID);
            if (existing == null)
            {
                throw new EntityNotFoundException("La compañía");
            }

            try
            {
                existing.BusinessGroupID = company.BusinessGroupID > 0 ? company.BusinessGroupID : existing.BusinessGroupID;
                existing.CountryID = company.CountryID > 0 ? company.CountryID : existing.CountryID;
                existing.StateID = company.StateID > 0 ? company.StateID : existing.StateID;
                existing.Name = company.Name != null ? company.Name : existing.Name;
                existing.Description = company.Description != null ? company.Description : existing.Description;
                existing.Nit = company.Nit != null ? company.Nit : existing.Nit;
                existing.Address = company.Address != null ? company.Address : existing.Address;
                existing.Phone = company.Phone != null ? company.Phone : existing.Phone;
                existing.IsVisible = company.IsVisible;

                existing.ModifiedBy = company.ModifiedBy != null ? company.ModifiedBy : existing.ModifiedBy;
                existing.ModifiedAt = DateTime.Now;

                await _companyRepository.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la compañía: {ex.Message}");
                throw new InvalidOperationException("Error al actualizar la compañía", ex);
            }
        }

        #region License Management

        public async Task<bool> AssignLicensesToCompanyAsync(int companyId, int totalLicenses, string assignedBy)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
            {
                throw new EntityNotFoundException("La compañía no existe");
            }

            var existingLicense = await _companyLicenseRepository.GetLicenseByCompanyIdAsync(companyId);
            
            if (existingLicense != null)
            {
                // Si ya existe el registro, aumenta el total
                existingLicense.AllocatedLicenses += totalLicenses;
                existingLicense.ModifiedBy = assignedBy;
                existingLicense.ModifiedAt = DateTime.Now;

                await _companyLicenseRepository.UpdateAsync(existingLicense);

                //var updateLog = new CompanyLicenseLog(
                //    companyLicenseID: existingLicense.CompanyLicenseID,
                //    actionType: "assign",
                //    quantity: totalLicenses,
                //    relatedExamResultID: 0,
                //    createdBy: assignedBy
                //);

                //await _companyLicenseLogRepository.AddAsync(updateLog);

                _logger.LogInformation($"Se agregaron {totalLicenses} licencias adicionales a la compañía {companyId} por {assignedBy}. Total: {existingLicense.AllocatedLicenses}");
                return true;
            }

            // Si no existe, crea el registro nuevo
            var companyLicense = new CompanyLicense(
                companyID: companyId,
                businessGroupLicenseID: 0,
                allocatedLicenses: totalLicenses,
                createdBy: assignedBy,
                modifiedBy: assignedBy
            );

            await _companyLicenseRepository.AddAsync(companyLicense);

            //var licenseLog = new CompanyLicenseLog(
            //    companyLicenseID: companyLicense.CompanyLicenseID,
            //    actionType: "assign",
            //    quantity: totalLicenses,
            //    relatedExamResultID: 0,
            //    createdBy: assignedBy
            //);

            //await _companyLicenseLogRepository.AddAsync(licenseLog);
            _logger.LogInformation($"Se asignaron {totalLicenses} licencias nuevas a la compañía {companyId} por {assignedBy}");

            return true;
        }

        public async Task<bool> ConsumeLicenseAsync(int companyId, int examResultId, string consumedBy)
        {
            var companyLicense = await _companyLicenseRepository.GetLicenseByCompanyIdAsync(companyId);
            if (companyLicense == null)
            {
                throw new EntityNotFoundException("La compañía no tiene licencias asignadas");
            }

            if (companyLicense.UsedLicenses >= companyLicense.AllocatedLicenses)
            {
                throw new InvalidOperationException("No hay licencias disponibles para consumir");
            }

            // Actualiza el uso genérico de licencias
            companyLicense.UsedLicenses++;
            companyLicense.ModifiedBy = consumedBy;
            companyLicense.ModifiedAt = DateTime.Now;

            await _companyLicenseRepository.UpdateAsync(companyLicense);

            // Guarda el log de consumo
            //var licenseLog = new CompanyLicenseLog(
            //    companyLicenseID: companyLicense.CompanyLicenseID,
            //    actionType: "consume",
            //    quantity: 1,
            //    relatedExamResultID: examResultId,
            //    createdBy: consumedBy
            //);

            //await _companyLicenseLogRepository.AddAsync(licenseLog);

            _logger.LogInformation($"Licencia consumida para la compañía {companyId}, examen resultado {examResultId} por {consumedBy}. Licencias usadas: {companyLicense.UsedLicenses}/{companyLicense.AllocatedLicenses}");

            return true;
        }

        public async Task<CompanyLicense?> GetCompanyLicensesAsync(int companyId)
        {
            return await _companyLicenseRepository.GetLicenseByCompanyIdAsync(companyId);
        }

        public async Task<List<CompanyLicense>> GetAllCompanyLicensesAsync()
        {
            return await _companyLicenseRepository.GetAllLicenseAsync();
        }

        public async Task<int> GetAvailableLicensesAsync(int companyId)
        {
            var companyLicense = await _companyLicenseRepository.GetLicenseByCompanyIdAsync(companyId);
            if (companyLicense == null)
            {
                return 0;
            }

            return companyLicense.AllocatedLicenses - companyLicense.UsedLicenses;
        }

        #endregion
    }
}
