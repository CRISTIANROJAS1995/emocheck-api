using Application.DTOs.Area;
using Application.DTOs.State;
using Domain.Entities;

public static class AreaMapper
{
    public static AreaDto ToDto(Area entity)
    {
        return new AreaDto
        {
            AreaID = entity.AreaID,
            Name = entity.Name,
            Description = entity.Description,
            IsVisible = entity.IsVisible,
            Company = CompanyMapper.ToDto(entity.Company),
            State = new StateDto
            {
                StateID = entity.State.StateID,
                Name = entity.State.Name
            }
        };
    }
}
