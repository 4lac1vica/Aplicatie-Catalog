using AplicatieCatalog.Data;
using AplicatieCatalog.Models;
using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace AplicatieCatalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string term)
        {
            List<ApplicationUserViewModel> users = new();

            if (!string.IsNullOrWhiteSpace(term))
            {
                users = await _userManager.Users
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
            }

            ViewBag.Term = term;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["Error"] = "Userul nu exista.";
                return RedirectToAction("SearchUsers");
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student != null)
            {
                _context.Students.Remove(student);
            }

            var profesor = await _context.Profesori.FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

            if (profesor != null)
            {
                _context.Profesori.Remove(profesor);
            }

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Stergerea a esuat!";
            }

            return RedirectToAction("SearchUsers");
        }

        [HttpGet]
        public IActionResult AddMaterie()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AddMaterie(AddMaterieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exista = await _context.Materii.AnyAsync(m => m.Nume == model.Nume);

            if (exista)
            {
                ModelState.AddModelError("Nume", "Materia exista deja!!");
                return View(model);
            }

            var materie = new Materie
            {
                Nume = model.Nume
            };

            _context.Materii.Add(materie);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Materia a fost adaugata!!";
            return RedirectToAction("AddMaterie");


        }

        [HttpGet]
        public async Task<IActionResult> StergeMaterie()
        {
            var materii = await _context.Materii.ToListAsync();
            return View(materii);
        }


        [HttpPost]
        public async Task<IActionResult> StergeMaterie(int MaterieId)
        {

            var materie = await _context.Materii.FirstOrDefaultAsync(m => m.Id == MaterieId);
            
            if (materie == null)
            {
                TempData["Error"] = "Materia nu exista!";
                return RedirectToAction("Index");
            }

            _context.Materii.Remove(materie);
            await _context.SaveChangesAsync();

            TempData["Succes"] = "Materia a fost stearsa!";
            return RedirectToAction("Index");

        }



    }
}