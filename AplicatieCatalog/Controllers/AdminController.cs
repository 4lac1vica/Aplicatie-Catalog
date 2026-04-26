using AplicatieCatalog.Data;
using AplicatieCatalog.Models;
using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace AplicatieCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> SearchUsers(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Ok(new List<ApplicationUserViewModel>());
            }

            var users = await _userManager.Users
                .Where(u =>
                    u.Email.Contains(term) ||
                    u.FirstName.Contains(term) ||
                    u.LastName.Contains(term))
                .Select(u => new ApplicationUserViewModel
                {
                    Id = u.Id,
                    Email = u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "Userul nu exista!" });

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student != null)
            {
                _context.Students.Remove(student);
            }

            var profesor = await _context.Profesori
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

            if (profesor != null)
            {
                _context.Profesori.Remove(profesor);
            }

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(new { message = "Stergerea a esuat!" });

            return Ok(new { message = "Userul a fost sters cu succes!" });

        }

        [HttpGet("materii")]
        public async Task<IActionResult> GetMaterii()
        {
            var materii = await _context.Materii
                .Select(m => new
                {
                    id = m.Id,
                    nume = m.Nume
                })
                .ToListAsync();

            return Ok(materii);
        }

        [HttpPost("materii")]
        public async Task<IActionResult> AddMaterie([FromBody] AddMaterieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exista = await _context.Materii
                .AnyAsync(m => m.Nume == model.Nume);

            if (exista)
            {
                return BadRequest(new { message = "Materia exista deja!" });
            }

            var materie = new Materie
            {
                Nume = model.Nume
            };

            _context.Materii.Add(materie);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Materia a fost deja adaugata!" });
        }

        [HttpDelete("materii/{materieId}")]
        public async Task<IActionResult> DeleteMaterie(int materieId)
        {
            var materie = await _context.Materii
                .FirstOrDefaultAsync(m => m.Id == materieId);

            if (materie == null)
            {
                return NotFound(new { message = "Materia nu exista!" });
            }

            _context.Materii.Remove(materie);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Materia a fost stearsa!" });
        }
    }
}