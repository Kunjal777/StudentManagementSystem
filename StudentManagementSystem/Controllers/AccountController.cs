using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Teacher teacher)
        {
            if(string.IsNullOrWhiteSpace(teacher.Name) || string.IsNullOrWhiteSpace(teacher.Username) ||string.IsNullOrWhiteSpace(teacher.Password) )
            {
                ViewBag.Error = "all field are required";
                return View(teacher);
            }
            bool exists = AppData.Teachers.Any(t => t.Username.Equals(
                    teacher.Username,
                    StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                ViewBag.Error = "Username already exists.";
                return View(teacher);
            }

            AppData.Teachers.Add(teacher);

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var teacher = AppData.Teachers.FirstOrDefault(t =>
                t.Username == username &&
                t.Password == password);

            if (teacher == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            HttpContext.Session.SetString(
                "TeacherUsername", teacher.Username);

            return RedirectToAction("Index", "Student");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
