using Application.DTOs.Role.Rl;

namespace Application.DTOs.Role
{
    public class RoleDto
    {
        public int RoleID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<RolePermissionDto> Permissions { get; set; }
    }
}
