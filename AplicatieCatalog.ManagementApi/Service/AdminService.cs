using AplicatieCatalog.Data;
using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;





namespace AplicatieCatalog.ManagementApi.Service
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<UserAdmin>> SearchUsersAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<UserAdmin>();

            term = term.ToLower();

            return await _context.Users
                .Where(u => u.Email != null &&
                u.Email.ToLower().Contains(term))
                .Select(u => new UserAdmin
                {
                    Id = u.Id,
                    Email = u.Email
                })
                .ToListAsync();
        }


        public async Task deleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("Utilizatorul nu exista!");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                throw new Exception("Nu poti sterge un administrator!");

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student != null)
            {
                var studentGrades = await _context.Grades
                    .Where(g => g.StudentId == student.ID)
                    .ToListAsync();

                if (studentGrades.Any())
                    _context.Grades.RemoveRange(studentGrades);

                _context.Students.Remove(student);
            }

            var profesor = await _context.Profesori
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

            if (profesor != null)
            {
                var profesorGrades = await _context.Grades
                    .Where(g => g.ProfesorId == profesor.ID)
                    .ToListAsync();

                if (profesorGrades.Any())
                    _context.Grades.RemoveRange(profesorGrades);

                _context.Profesori.Remove(profesor);
            }

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }


        public async Task addMaterie(AddMaterie dto)
        {
            if (string.IsNullOrEmpty(dto.Nume))
                throw new Exception("Numele materiei noi este obligatoriu!");

            var exists = await _context.Materii.AnyAsync(m => m.Nume == dto.Nume);

            if (exists)
            {
                throw new Exception("Materia exista deja!!");
            }

            var materie = new Materie
            {
                Nume = dto.Nume
            };

            _context.Materii.Add(materie);
            await _context.SaveChangesAsync();

        }


    }
}
