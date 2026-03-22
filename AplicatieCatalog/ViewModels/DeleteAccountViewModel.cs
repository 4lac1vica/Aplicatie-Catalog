using System.ComponentModel.DataAnnotations;


namespace AplicatieCatalog.ViewModels
{
    public class DeleteAccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma Parola")]
        public string Password { get; set; }
    }
}
