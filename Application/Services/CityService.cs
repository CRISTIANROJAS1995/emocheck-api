using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<CityService> _logger;

        public CityService(ICityRepository cityRepository, ILogger<CityService> logger)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task<List<City>> GetCitiesByUserIdAsync(int userId)
        {
            return await _cityRepository.GetCitiesByUserIdAsync(userId);
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _cityRepository.GetAllCitiesAsync();
        }
    }
}
