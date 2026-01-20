using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Subject
{
    public class CreatedSubjectDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identification { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El cliente debe ser mayor que 0.")]
        public int ClientId { get; set; }
    }
}
