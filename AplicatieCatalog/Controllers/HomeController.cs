using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieCatalog.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                message = "Bine ai venit in aplicatia catalog!",
                page = "Home"
            });

        }

        [Authorize(Roles = "Student")]
        [HttpGet("student")]
        public IActionResult StudentMain()
        {
            return Ok(new
            {
                meesage = "Bine te-am regasit, student!",
                role = "Student",
                page = "Student Dashboard"
            });
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("teacher")]
        public IActionResult TeacherMain()
        {
            return Ok(new
            {
                message = "Bine v-am regaasit, profesor!",
                role = "Teacher",
                page = "Teacher Dashboard"
            });
        }

        [HttpGet("privacy")]

        public IActionResult Privacy()
        {
            return Ok(new
            {
                title = "Privacy",
                content = "Pagina de privacy pentru aplicatie!"
            });
        }

        [HttpGet("about-us")]
        public IActionResult AboutUs()
        {
            return Ok(new
            {
                title = "About Us",
                content = "Aplicatie pentru student si profesori."
            });
        }
    }
}