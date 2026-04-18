using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;



namespace AplicatieCatalog.ViewModels
{
    public class AddGradesViewModel
    {
        [Required(ErrorMessage = "Selecteaza studentul.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Selecteaza Materia")]
        public int MaterieId { get; set; }

        [Required(ErrorMessage = "Introduceti nota")]
        [Range(1, 10, ErrorMessage = "Nota trebuie sa fie in intervalul 1 - 10")]
        public int Valoare { get; set; }

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Materii { get; set; } = new();

    }
}
