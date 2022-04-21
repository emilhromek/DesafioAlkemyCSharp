using System.ComponentModel.DataAnnotations;

namespace DesafioAlkemyCSharp.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Se requiere nombre de usuario")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Se requiere e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se requiere password")]
        public string Password { get; set; }
    }
}
