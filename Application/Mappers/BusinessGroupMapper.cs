using Application.DTOs.BusinessGroup;
using Domain.Entities;

public static class BusinessGroupMapper
{
    public static BusinessGroupDto ToDto(BusinessGroup entity)
    {
        return new BusinessGroupDto
        {
            BusinessGroupID = entity.BusinessGroupID,
            Name = entity.Name,
            Description = entity.Description,
            IsVisible = entity.IsVisible
        };
    }
}
