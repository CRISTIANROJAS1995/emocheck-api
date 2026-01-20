using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly ILogger<AreaService> _logger;

        public AreaService(IAreaRepository areaRepository, ILogger<AreaService> logger)
        {
            _areaRepository = areaRepository;
            _logger = logger;
        }

        public async Task<List<Area>> GetByUserIdAsync(int userId)
        {
            return await _areaRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Area>> GetByIdsAsync(List<int> areaIds)
        {
            return await _areaRepository.GetByIdsAsync(areaIds);
        }
    }
}
