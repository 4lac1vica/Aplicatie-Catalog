using AplicatieCatalog.ManagementApi.DTOs;

namespace AplicatieCatalog.ManagementApi.Service
{
    public interface IAddGrades
    {
        Task AddNotaAsync(AddGradeDTO dto);

    }
}
