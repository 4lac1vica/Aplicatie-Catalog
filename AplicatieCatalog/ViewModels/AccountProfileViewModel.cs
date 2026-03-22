


using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AplicatieCatalog.ViewModels
{
    public class AccountProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Role { get; set; }

        public string? Materie { get; set; }
        public string? Grupa { get; set; }
    }
}
