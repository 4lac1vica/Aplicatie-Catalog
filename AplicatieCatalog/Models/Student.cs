namespace AplicatieCatalog.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }

        public string email { get; set; }
        public string parolaHash { get; set; }

        public int numarGrupa { get; set; }

        public string specializare { get; set; }

        public string telefon { get; set; }

        public string adresaOrigine { get; set; }

        public string adresaCurenta { get; set; }

        public int varsta { get; set; }
    }
}
