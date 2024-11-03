using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class AdminController : Controller
    {
        private MyContext MyContext;
        public AdminController(MyContext context)
        {
            MyContext = context;
        }
        public IActionResult Index()
        {
            var result=HttpContext.Session.GetString("AdminEmail");
            if (result == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string AdminEmail,string AdminPassword)
        {
            var raw = MyContext.Admins.Where(a => a.admin_email == AdminEmail && a.admin_password == AdminPassword).FirstOrDefault();
            if (raw != null)
            {
                HttpContext.Session.SetString("AdminEmail", raw.admin_id.ToString());
                return RedirectToAction("Index");
            }
            else
            {

                return View();
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return View();
        }
    }
}
