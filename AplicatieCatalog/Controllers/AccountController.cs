using AplicatieCatalog.Data;
using AplicatieCatalog.Models;
using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AplicatieCatalog.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (model.Role == "Teacher" && string.IsNullOrWhiteSpace(model.Materie))
            {
                ModelState.AddModelError("Materie", "Materie este obligatorie pentru profesor.");
            }

            if (!ModelState.IsValid)
                return View(model);

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
            {
                ViewBag.Error = "Registration failed!";
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            if (model.Role == "Student")
            {
                var student = new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Telefon = model.Telephone,
                    Grupa = model.Grupa,
                    ApplicationUserId = user.Id
                };

                _context.Students.Add(student);
            }
            else if (model.Role == "Teacher")
            {
                var profesor = new Profesor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Telefon = model.Telephone,
                    Materie = model.Materie,
                    ApplicationUserId = user.Id
                };

                _context.Profesori.Add(profesor);
            }

            await _context.SaveChangesAsync();

            ViewBag.Success = "Account created successfully!";
            return View();
        }

        [HttpGet]
        public IActionResult Login(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials!";
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Invalid credentials!";
                return View(model);
            }

            if (model.Role == "Student")
                return RedirectToAction("StudentMain", "Home");

            if (model.Role == "Teacher")
                return RedirectToAction("TeacherMain", "Home");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AccountProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var model = new AccountProfileViewModel
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
                {
                    model.Grupa = student.Grupa;
                }
            }

            else if (role == "Teacher")
            {
                var profesor = await _context.Profesori
                    .FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

                if (profesor != null)
                {
                    model.Materie = profesor.Materie;
                }
            }

            return View(model);

        }


        [Authorize]
        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                ModelState.AddModelError("Password", "Wrong Password.");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Student")
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

                if (student != null)
                {
                    _context.Students.Remove(student);
                }
            }

            else if (role == "Teacher")
            {
                var profesor = await _context.Profesori.FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

                if (profesor != null)
                {
                    _context.Profesori.Remove(profesor);
                }
            }

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Eroare", error.Description);
                }

                return View(model);
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Telephone = user.PhoneNumber,
                Role = role
            };

            if (role == "Student")
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

                if (student != null)
                {
                    model.Grupa = student.Grupa;
                }
            }
            else if (role == "Teacher")
            {
                var profesor = await _context.Profesori
                    .FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

                if (profesor != null)
                {
                    model.Materie = profesor.Materie;
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                ModelState.AddModelError("Password", "Parola este incorecta.");
                return View(model);
            }

            
            user.PhoneNumber = model.Telephone;

            var updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                foreach (var error in updateUserResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }


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

            return RedirectToAction("AccountProfile");
        }

    }
    
}