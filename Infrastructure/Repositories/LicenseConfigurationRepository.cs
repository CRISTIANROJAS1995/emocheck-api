using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LicenseConfigurationRepository : ILicenseConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public LicenseConfigurationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LicenseConfiguration> GetByKeyAsync(string configKey)
        {
            return await _context.LicenseConfigurations
                .FirstOrDefaultAsync(c => c.ConfigKey == configKey)
                ?? throw new KeyNotFoundException($"Configuration key '{configKey}' not found");
        }

        public async Task<int> GetMaxOverdraftLicensesAsync()
        {
            var config = await _context.LicenseConfigurations.FirstOrDefaultAsync();
            return config?.MaxOverdraftLicenses ?? 0;
        }

        public async Task UpdateConfigValueAsync(string configKey, int newValue, string modifiedBy)
        {
            var config = await _context.LicenseConfigurations
                .FirstOrDefaultAsync(c => c.ConfigKey == configKey);

            if (config == null)
            {
                // Crear nueva configuración si no existe
                config = new LicenseConfiguration(
                    configKey: configKey,
                    configValue: newValue,
                    description: "Configuración de sobregiro de licencias",
                    createdBy: modifiedBy,
                    modifiedBy: modifiedBy
                );
                _context.LicenseConfigurations.Add(config);
            }
            else
            {
                config.UpdateValue(newValue, modifiedBy);
            }

            await _context.SaveChangesAsync();
        }

        // Implementación de IGenericRepository
        public async Task<LicenseConfiguration> GetByIdAsync(int id)
        {
            return await _context.LicenseConfigurations.FindAsync(id)
                ?? throw new KeyNotFoundException($"LicenseConfiguration with ID {id} not found");
        }

        public async Task<IEnumerable<LicenseConfiguration>> GetAllAsync()
        {
            return await _context.LicenseConfigurations.ToListAsync();
        }

        public async Task<LicenseConfiguration> GetAsync()
        {
            return await _context.LicenseConfigurations.FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("No license configuration found");
        }

        public async Task AddAsync(LicenseConfiguration entity)
        {
            _context.LicenseConfigurations.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LicenseConfiguration entity)
        {
            _context.LicenseConfigurations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.LicenseConfigurations.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
