using AplicatieCatalog.Enums;

namespace AplicatieCatalog.Models
{
    public class User
    {
        public int ID { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }

        public Roles rol { get; set; }

        public string email { get; set; }

        public string passwordHash { get; set; }

        
    }
}
