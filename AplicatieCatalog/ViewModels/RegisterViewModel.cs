using System.ComponentModel.DataAnnotations;


namespace AplicatieCatalog.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public string Role { get; set; }

        public string? Materie { get; set; }

        public string? Grupa { get; set; }
    }
}
