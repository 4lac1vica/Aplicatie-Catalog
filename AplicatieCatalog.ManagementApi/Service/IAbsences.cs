using AplicatieCatalog.ManagementApi.DTOs;

namespace AplicatieCatalog.ManagementApi.Service
{
    public interface IAbsences
    {
        Task AddAbsencesAsync(AddAbsenceDTO dto);
        Task<List<StudentAbsencesDTO>> GetStudentAbsencesAsync(int studentId);
    }
}
