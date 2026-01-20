using Application.DTOs.Company;
using Application.DTOs.BusinessGroup;
using Application.DTOs.Country;
using Application.DTOs.State;
using Domain.Entities;

public static class CompanyMapper
{
    public static CompanyDto ToDto(Company entity)
    {
        return new CompanyDto
        {
            CompanyID = entity.CompanyID,
            Name = entity.Name,
            Description = entity.Description,
            Nit = entity.Nit,
            Address = entity.Address,
            Phone = entity.Phone,
            BusinessGroup = new BusinessGroupDto
            {
                BusinessGroupID = entity.BusinessGroup.BusinessGroupID,
                Name = entity.BusinessGroup.Name,
                Description = entity.BusinessGroup.Description,
                IsVisible = entity.BusinessGroup.IsVisible
            },
            Country = new CountryDto
            {
                CountryID = entity.Country.CountryID,
                Name = entity.Country.Name
            },
            State = new StateDto
            {
                StateID = entity.State.StateID,
                Name = entity.State.Name
            },
            IsVisible = entity.IsVisible
        };
    }

    public static List<CompanyDto> ToDtoList(List<Company> companies)
    {
        return companies.Select(company => ToDto(company)).ToList();
    }
}
