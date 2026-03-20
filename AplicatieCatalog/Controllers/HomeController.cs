using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieCatalog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult StudentMain()
        {
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult TeacherMain()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}