using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AplicatieCatalog.ViewModels
{
    public class AbsentaViewModel
    {
        [Required(ErrorMessage = "Selectati studentul.")]   
        
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Selectati materia.")]
        public int MaterieId { get; set; }
        public DateTime Data { get; set; }

        public string? Materie { get; set; }
        public string? Profesor { get; set; }

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Materii { get; set; } = new();
    }
}
