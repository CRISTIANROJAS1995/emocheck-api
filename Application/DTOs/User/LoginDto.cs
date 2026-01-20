using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
