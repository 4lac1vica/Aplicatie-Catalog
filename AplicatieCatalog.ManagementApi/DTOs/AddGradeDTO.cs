namespace AplicatieCatalog.ManagementApi.DTOs
{
    public class AddGradeDTO
    {
        public int StudentId { get; set; }
        public int MaterieId { get; set; }
        public int Valoare { get; set; }

        public int ProfesorId { get; set; }
    }
}
