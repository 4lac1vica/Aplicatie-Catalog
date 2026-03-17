using System.Runtime.CompilerServices;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AplicatieCatalog.Models
{
    public class Profesor
    {
       public int ID { get; set; }
       public string lastName { get; set; }
       
       public string firstName { get; set; }
       
       public string telefon { get; set; }
   
       public string email { get; set; }
       
       public string materie { get; set; }
       
       public string adresa { get; set; }
       
       public string parolaHash { get; set; }
       
       public int varsta { get; set; }
       
    }
}
