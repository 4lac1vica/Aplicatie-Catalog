using System.ComponentModel.DataAnnotations;


namespace AplicatieCatalog.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        [MinLength(10, ErrorMessage = "Numarul de telefon trebuie sa fie valid!")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        
        public string Role { get; set; }

        public string? Materie { get; set; }

        public string? Grupa { get; set; }
    }
}
