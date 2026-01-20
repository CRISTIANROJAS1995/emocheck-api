using Application.DTOs.Area;
using Application.DTOs.BusinessGroup;
using Application.DTOs.City;
using Application.DTOs.Company;
using Application.DTOs.Country;
using Application.DTOs.Permission;
using Application.DTOs.Role;
using Application.DTOs.Role.Rl;
using Application.DTOs.State;
using Application.DTOs.User;
using Application.DTOs.User.Rl;
using Domain.Entities;
using Domain.Enums;

public static class UserMapper
{
    public static User ToEntity(CreatedUserDto userDto, string createdBy)
    {
        return new User(
            (int)StateEnum.Active,
            userDto.Email,
            userDto.Password,
            userDto.FullName     
        )
        {
            CreatedBy = createdBy,
            ModifiedBy = createdBy,
            Phone = string.Empty,
            Address = string.Empty,
            ProfileImage = string.Empty
        };
    }

    public static User ToEntity(UpdateUserDto userDto, string updateBy)
    {
        var user = new User(
            userDto.StateID,
            userDto.Email!,
            userDto.Password!,
            userDto.FullName!
        )
        {
            ModifiedBy = updateBy,
            ModifiedAt = DateTime.Now,
            Phone = userDto.Phone!,
            Address = userDto.Address!
        };

        return user;
    }

    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            UserID = user.UserID,
            State = new StateDto
            {
                StateID = user.State.StateID,
                Name = user.State.Name
            },
            Email = user.Email,
            Password = user.Password,
            FullName = user.FullName,
            Phone = user.Phone,
            Address = user.Address,
            Avatar = string.Empty,
            Areas = user.UserAreas != null
                ? user.UserAreas.Select(edt => new UserAreaDto
                {
                    Area = new AreaDto
                    {
                        AreaID = edt.Area.AreaID,
                        Company = new CompanyDto 
                        {
                            CompanyID = edt.Area.Company.CompanyID,
                            BusinessGroup = new BusinessGroupDto
                            {
                                BusinessGroupID = edt.Area.Company.BusinessGroup.BusinessGroupID,
                                Name = edt.Area.Company.BusinessGroup.Name,
                                Description = edt.Area.Company.BusinessGroup.Description,
                                IsVisible = edt.Area.Company.BusinessGroup.IsVisible
                            },
                            Country = new CountryDto
                            {
                                CountryID = edt.Area.Company.Country.CountryID,
                                Name = edt.Area.Company.Country.Name
                            },
                            State = new StateDto
                            {
                                StateID = edt.Area.Company.State.StateID,
                                Name = edt.Area.Company.State.Name
                            },
                            Name = edt.Area.Company.Name,
                            Description = edt.Area.Company.Description,
                            Nit = edt.Area.Company.Nit,
                            Address = edt.Area.Company.Address,
                            Phone = edt.Area.Company.Phone
                        },
                        State = new StateDto
                        {
                            StateID = edt.Area.State.StateID,
                            Name = edt.Area.State.Name,
                        },
                        Name = edt.Area.Name,
                        Description = edt.Area.Description,
                        IsVisible = edt.Area.IsVisible
                    }
                }).ToList()
                : new List<UserAreaDto>(),
            Cities = user.UserCities != null
                ? user.UserCities.Select(edt => new UserCityDto
                {
                    City = new CityDto
                    {
                        CityID = edt.City.CityID,
                        Name = edt.City.Name
                    }
                }).ToList()
                : new List<UserCityDto>(),
            Roles = user.UserRoles != null
                ? user.UserRoles.Select(edt => new UserRoleDto
                {
                    Role = new RoleDto
                    {
                        RoleID = edt.Role.RoleID,
                        Name = edt.Role.Name,
                        Description = edt.Role.Description,
                        Permissions = edt.Role.RolePermissions != null
                            ? edt.Role.RolePermissions.Select(edtc => new RolePermissionDto
                            {
                                Permission = new PermissionDto
                                {
                                    PermissionID = edtc.Permission.PermissionID,
                                    Name = edtc.Permission.Name,
                                    Description = edtc.Permission.Description
                                }
                            }).ToList()
                            : new List<RolePermissionDto>(),
                    }
                }).ToList()
                : new List<UserRoleDto>(),
        };
    }

    public static List<UserDto> ToDtoList(List<User> users)
    {
        return users.Select(user => ToDto(user)).ToList();
    }
}
