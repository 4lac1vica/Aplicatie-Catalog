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
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (model.Role == "Teacher" && string.IsNullOrWhiteSpace(model.Materie))
                ModelState.AddModelError("Materie", "Campul 'Materie' este obligatoriu pentru profesor.");

            if (model.Role == "Student" && string.IsNullOrWhiteSpace(model.Grupa))
                ModelState.AddModelError("Grupa", "Campul 'Grupa' este obligatoriu pentru student.");

            if (model.Role == "Student" && !model.Email.EndsWith("@student.com"))
                ModelState.AddModelError("Email", "Pentru Student, emailul trebuie sa aiba terminatia @student.com.");

            if (model.Role == "Teacher" && !model.Email.EndsWith("@teacher.com"))
                ModelState.AddModelError("Email", "Pentru Teacher, emailul trebuie sa aiba terminatia @teacher.com.");

            if (model.Role != "Student" && model.Role != "Teacher")
                ModelState.AddModelError("Role", "Acest rol nu exista.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Telephone
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest(roleResult.Errors);
            }

            if (model.Role == "Student")
            {
                _context.Students.Add(new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Telefon = model.Telephone,
                    Grupa = model.Grupa,
                    ApplicationUserId = user.Id
                });
            }
            else if (model.Role == "Teacher")
            {
                _context.Profesori.Add(new Profesor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Telefon = model.Telephone,
                    Materie = model.Materie,
                    ApplicationUserId = user.Id
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cont creat cu succes!",
                role = model.Role
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized(new { message = "Email sau parola gresita." });

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: true,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
                return Unauthorized(new { message = "Email sau parola gresita." });

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            return Ok(new
            {
                message = "Autentificare reusita.",
                userId = user.Id,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                role = role
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new { message = "Logout reusit." });
        }

       
        [HttpGet("profile")]
        public async Task<IActionResult> AccountProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized(new { message = "Userul nu este autentificat." });

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var profile = new AccountProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.PhoneNumber,
                Role = role
            };

            if (role == "Student")
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

                if (student != null)
                    profile.Grupa = student.Grupa;
            }
            else if (role == "Teacher")
            {
                var profesor = await _context.Profesori
                    .FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

                if (profesor != null)
                    profile.Materie = profesor.Materie;
            }

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized(new { message = "Userul nu este autentificat." });

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
                return BadRequest(new { message = "Parola este incorecta." });

            user.PhoneNumber = model.Telephone;

            var updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
                return BadRequest(updateUserResult.Errors);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Student")
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

                if (student != null)
                {
                    student.Telefon = model.Telephone;
                    student.Grupa = model.Grupa;
                }
            }
            else if (role == "Teacher")
            {
                var profesor = await _context.Profesori
                    .FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

                if (profesor != null)
                {
                    profesor.Telefon = model.Telephone;
                    profesor.Materie = model.Materie;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profil actualizat cu succes." });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountViewModel model)
        {
            if (model == null ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { message = "Email si parola sunt obligatorii." });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound(new { message = "Userul nu exista." });

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
                return BadRequest(new { message = "Parola este incorecta." });

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

            if (student != null)
            {
                var grades = await _context.Grades
                    .Where(g => g.StudentId == student.ID)
                    .ToListAsync();

                _context.Grades.RemoveRange(grades);
                _context.Students.Remove(student);
            }

            var profesor = await _context.Profesori
                .FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

            if (profesor != null)
            {
                var grades = await _context.Grades
                    .Where(g => g.ProfesorId == profesor.ID)
                    .ToListAsync();

                _context.Grades.RemoveRange(grades);
                _context.Profesori.Remove(profesor);
            }

            await _context.SaveChangesAsync();

            var deleteUserResult = await _userManager.DeleteAsync(user);

            if (!deleteUserResult.Succeeded)
                return BadRequest(deleteUserResult.Errors);

            await _signInManager.SignOutAsync();

            return Ok(new { message = "Cont sters cu succes." });
        }
    }
}