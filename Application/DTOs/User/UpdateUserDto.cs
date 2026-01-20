using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UpdateUserDto
    {
        //[Required(ErrorMessage = "El UserID es obligatorio.")]
        //[Range(1, int.MaxValue, ErrorMessage = "El UserID debe ser mayor que 0.")]
        //public int UserID { get; set; }

        public int StateID { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
        public List<int>? Areas { get; set; }
        public List<int>? Cities { get; set; }
        public List<int>? Roles { get; set; }
    }
}
