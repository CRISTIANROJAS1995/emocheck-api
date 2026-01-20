using Domain.Interfaces.Services;
using Application.DTOs.User;
using Microsoft.Extensions.Logging;

namespace Application.Facades
{
    public interface IUserHierarchyFacade
    {
        Task<UserHierarchyDto> GetUserHierarchyAsync(int userId);
    }

    public class UserHierarchyFacade : IUserHierarchyFacade
    {
        private readonly IBusinessGroupService _businessGroupService;
        private readonly ICompanyService _companyService;
        private readonly IAreaService _areaService;
        private readonly ICityService _cityService;
        private readonly ILogger<UserHierarchyFacade> _logger;

        public UserHierarchyFacade(
            IBusinessGroupService businessGroupService,
            ICompanyService companyService,
            IAreaService areaService,
            ICityService cityService,
            ILogger<UserHierarchyFacade> logger)
        {
            _businessGroupService = businessGroupService;
            _companyService = companyService;
            _areaService = areaService;
            _cityService = cityService;
            _logger = logger;
        }

        public async Task<UserHierarchyDto> GetUserHierarchyAsync(int userId)
        {
            var businessGroups = await _businessGroupService.GetByUserIdAsync(userId);
            var companies = await _companyService.GetByUserIdAsync(userId);
            var areas = await _areaService.GetByUserIdAsync(userId);
            var cities = await _cityService.GetCitiesByUserIdAsync(userId);

            // Si no tiene restricciones por ciudad, devolver TODAS las ciudades
            if (cities == null || cities.Count == 0)
            {
                cities = await _cityService.GetAllCitiesAsync();
            }

            return new UserHierarchyDto
            {
                BusinessGroups = businessGroups.Select(BusinessGroupMapper.ToDto).ToList(),
                Companies = companies.Select(CompanyMapper.ToDto).ToList(),
                Areas = areas.Select(AreaMapper.ToDto).ToList(),
                Cities = cities.Select(CityMapper.ToDto).ToList()
            };
        }
    }
}
