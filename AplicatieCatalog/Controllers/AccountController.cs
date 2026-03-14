using System.Security;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieCatalog.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string LastName, string FirstName, string email, string password, string telephone ,string role)
        {

            if (string.IsNullOrWhiteSpace(LastName) ||
               string.IsNullOrWhiteSpace(FirstName) ||
               string.IsNullOrWhiteSpace(email) ||
               string.IsNullOrWhiteSpace(password) ||
               string.IsNullOrWhiteSpace(telephone) ||
               string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }


            string lowerEmail = (email ?? "").ToLower();
            string lowerLastName = (LastName ?? "").ToLower();
            string lowerFirstName = (FirstName ?? "").ToLower();

            if (role == "Student" &&
                (!lowerEmail.Contains("@student.com") ||
                 !lowerEmail.Contains(lowerLastName) ||
                 !lowerEmail.Contains(lowerFirstName)))
            {
                ViewBag.Error = "A student account must contain the university standard (firstname.lastname@student.com).";
                return View();
            }

            if (role == "Teacher" &&
                (!lowerEmail.Contains("@teacher.com") ||
                 !lowerEmail.Contains(lowerLastName) ||
                 !lowerEmail.Contains(lowerFirstName)))
            {
                ViewBag.Error = "A teacher account must contain the university standard (firstname.lastname@teacher.com).";
                return View();
            }

           

            ViewBag.Success = "Account created successfully!";
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password, string role)
        {

            ViewBag.Role = role;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Invalid credentials!";
                return View();
            }

            ViewBag.Success = "Success!";
            return View();
        }
    }
}