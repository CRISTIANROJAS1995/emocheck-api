using Application.DTOs.State;
using Application.DTOs.User.Rl;

namespace Application.DTOs.User
{
    public class UserDto
    {
        public int UserID { get; set; }
        public StateDto State { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public List<UserAreaDto> Areas { get; set; }
        public List<UserCityDto> Cities { get; set; }
        public List<UserRoleDto> Roles { get; set; }
    }
}
