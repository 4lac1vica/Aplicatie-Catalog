using AplicatieCatalog.Data;
using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AplicatieCatalog.ManagementApi.Service
{
    public class AddGradeService : IAddGrades
    {
        private readonly ApplicationDbContext _context;

        public AddGradeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNotaAsync(AddGradeDTO dto)
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
                throw new Exception("Nu exista materia respectiva!");
            }

            if (dto.Valoare < 1 || dto.Valoare > 10)
            {
                throw new Exception("Nota nu este valida!!");
            }

            var nota = new Nota
            {
                StudentId = dto.StudentId,
                ProfesorId = dto.ProfesorId,
                MaterieId = dto.MaterieId,
                Valoare = dto.Valoare
            };

            _context.Grades.Add(nota);

            await _context.SaveChangesAsync();
            
        }

        public async Task<List<StudentGradeDTO>> GetStudentGradesAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.Materie)
                .Include(g => g.Profesor)
                .Select(g => new StudentGradeDTO
                {
                    Valoare = g.Valoare,
                    Materie = g.Materie.Nume,
                    Profesor = g.Profesor.FirstName + " " + g.Profesor.LastName,
                    Data = g.Data
                })
                .ToListAsync();
        }
        
    }
}
