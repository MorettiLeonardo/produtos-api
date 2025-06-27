using System.ComponentModel.DataAnnotations;

namespace produtos_api.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
