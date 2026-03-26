using System.ComponentModel.DataAnnotations;

namespace AplicatieCatalog.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        [EmailAddress(ErrorMessage = "Email invalid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
