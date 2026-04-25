using System.ComponentModel.DataAnnotations;


namespace AplicatieCatalog.ViewModels
{
    public class DeleteAccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma Parola")]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirma Email")]
        public string Email { get; set; }
    }
}
