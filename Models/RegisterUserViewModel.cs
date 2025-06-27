using System.ComponentModel.DataAnnotations;

namespace produtos_api.Models
{
    public class RegisterUserViewModel
    {
        [EmailAddress(ErrorMessage = "Campo inválido")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas devem ser iguais")]
        public string ConfirmPassword { get; set; }
    }
}
