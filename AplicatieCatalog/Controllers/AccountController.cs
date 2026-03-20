using AplicatieCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieCatalog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
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
        public async Task<IActionResult> Register(
            string LastName,
            string FirstName,
            string email,
            string password,
            string telephone,
            string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = telephone,
                FirstName = FirstName,
                LastName = LastName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join(" ", result.Errors.Select(e => e.Description));
                return View();
            }

            
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

           
            await _userManager.AddToRoleAsync(user, role);

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
        public async Task<IActionResult> Login(string email, string password, string role)
        {
            ViewBag.Role = role;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials!";
                return View();
            }

            
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                ViewBag.Error = "Wrong role!";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Invalid credentials!";
                return View();
            }

           
            if (role == "Student")
                return RedirectToAction("StudentMain", "Home");

            if (role == "Teacher")
                return RedirectToAction("TeacherMain", "Home");

            return RedirectToAction("Index", "Home");
        }

        

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}