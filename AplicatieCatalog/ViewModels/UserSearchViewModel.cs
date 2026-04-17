using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AplicatieCatalog.ViewModels
{
    public class UserSearchViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Role { get; set; }

    }
}
