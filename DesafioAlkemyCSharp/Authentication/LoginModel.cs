using System.ComponentModel.DataAnnotations;

namespace DesafioAlkemyCSharp.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Se requiere nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Se requiere password")]
        public string Password { get; set; }
    }
}
