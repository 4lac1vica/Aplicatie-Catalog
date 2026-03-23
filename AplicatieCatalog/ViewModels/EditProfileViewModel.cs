using System.ComponentModel.DataAnnotations;

namespace AplicatieCatalog.ViewModels
{
    public class EditProfileViewModel
    {   
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Telephone { get; set; }
        public string? Grupa { get; set; }
        public string? Materie { get; set; }

        public string? Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma parola")]
        public string Password { get; set; } = string.Empty;
    }
}
