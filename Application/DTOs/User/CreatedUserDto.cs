using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class CreatedUserDto
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string FullName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
        public List<int> Areas { get; set; }
        public List<int> Cities { get; set; }
        public List<int> Roles { get; set; }
    }
}
