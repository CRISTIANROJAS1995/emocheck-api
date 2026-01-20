using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class BusinessGroupService : IBusinessGroupService
    {
        private readonly IBusinessGroupRepository _businessGroupRepository;
        private readonly ILogger<BusinessGroupService> _logger;

        public BusinessGroupService(IBusinessGroupRepository businessGroupRepository, ILogger<BusinessGroupService> logger)
        {
            _businessGroupRepository = businessGroupRepository;
            _logger = logger;
        }

        public async Task<List<BusinessGroup>> GetByUserIdAsync(int userId)
        {
            return await _businessGroupRepository.GetByUserIdAsync(userId);
        }
    }
}
