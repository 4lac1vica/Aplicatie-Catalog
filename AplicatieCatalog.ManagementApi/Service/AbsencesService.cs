using AplicatieCatalog.Controllers;
using AplicatieCatalog.Data;
using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace AplicatieCatalog.ManagementApi.Service
{
    public class AbsencesService : IAbsences
    {

        private readonly ApplicationDbContext _context;

        public AbsencesService (ApplicationDbContext context)
        {
            _context = context;
        }
        

        public async Task AddAbsencesAsync(AddAbsenceDTO dto)
        {
            var student = await _context.Students.FindAsync(dto.StudentId);
            
            if (student == null)
            {
                throw new Exception("Studentul nu exista!");
            }

            var profesor = await _context.Profesori.FindAsync(dto.ProfesorId);

            if (profesor == null)
            {
                throw new Exception("Profesorul nu exista!");
            }


            var materie = await _context.Materii.FindAsync(dto.MaterieId);

            if (materie == null)
            {
                throw new Exception("Materia nu exista!");
            }

            var absenta = new Absenta
            {
                StudentId = dto.StudentId,
                ProfesorId = dto.ProfesorId,
                MaterieId = dto.MaterieId
            };

            _context.Absente.Add(absenta);
            await _context.SaveChangesAsync();
        }


        public async Task<List<StudentAbsencesDTO>> GetStudentAbsencesAsync(int studentId)
        {
            return await _context.Absente
                .Where(a => a.StudentId == studentId)
                .Include(a => a.Materie)
                .Include(a => a.Profesor)
                .Select(a => new StudentAbsencesDTO
                {
                   Materie = a.Materie.Nume,
                   Profesor = a.Profesor.FirstName + " " + a.Profesor.LastName,
                   Data = a.Data
                })
                .ToListAsync();
        }









    }
}
