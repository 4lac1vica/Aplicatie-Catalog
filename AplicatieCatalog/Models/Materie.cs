namespace AplicatieCatalog.Models
{
    public class Materie
    {   
       public int Id { get; set; } 
       public string Nume { get; set; }
       
       public List<Nota> Note { get; set; } 

       public List<Absenta> Absente { get; set; } 
       
    }
}
