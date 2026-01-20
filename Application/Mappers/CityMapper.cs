using Application.DTOs.City;
using Domain.Entities;

public static class CityMapper
{
    public static CityDto ToDto(City entity)
    {
        return new CityDto
        {
            CityID = entity.CityID,
            Name = entity.Name
        };
    }
}
