namespace AplicatieCatalog.Models
{
    public class Absenta
    {

        public int ID { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public Profesor Profesor { get; set; }
        public int ProfesorId { get; set; }
        
        public Materie Materie { get; set; }
        
        public int MaterieId { get; set; }

        

    }
}
