namespace AplicatieCatalog.Models
{
    public class Profesor
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Materie { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
