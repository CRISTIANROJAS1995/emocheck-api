using Application.DTOs.Area;
using Application.DTOs.BusinessGroup;
using Application.DTOs.City;
using Application.DTOs.Company;

namespace Application.DTOs.User
{
    public class UserHierarchyDto
    {
        public List<BusinessGroupDto> BusinessGroups { get; set; } = [];
        public List<CompanyDto> Companies { get; set; } = [];
        public List<AreaDto> Areas { get; set; } = [];
        public List<CityDto> Cities { get; set; } = [];
    }
}
