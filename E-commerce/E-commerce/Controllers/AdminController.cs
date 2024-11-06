using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;

namespace E_commerce.Controllers
{
    public class AdminController : Controller
    {
        private MyContext MyContext;
        private IWebHostEnvironment _env;
        public AdminController(MyContext context,IWebHostEnvironment env)
        {
            MyContext = context;
            this._env = env;
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
            return RedirectToAction("Login");
        }
        public IActionResult Profile()
        {
            var adminIDString = HttpContext.Session.GetString("AdminEmail");
            if (adminIDString == null)
            {
                return RedirectToAction("Login");
            }

            if (int.TryParse(adminIDString, out int adminID))
            {
                var row = MyContext.Admins.Where(a => a.admin_id == adminID).ToList();
                return View(row);
            }

            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            MyContext.Admins.Update(admin);
            MyContext.SaveChanges(); 

            return RedirectToAction("Profile");
        }
        [HttpPost]
    public IActionResult ChangeImage(IFormFile admin_image, Admin admin)
        {


            // Define the file path
            string filePath = Path.Combine(_env.WebRootPath, "Images", admin_image.FileName);

            // Save the uploaded file to the server
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                admin_image.CopyTo(fs);
            }

            // Update the admin's image path
            admin.admin_image = admin_image.FileName;
            MyContext.Admins.Update(admin);
            MyContext.SaveChanges();


            return RedirectToAction("Profile");
        }

    }
}
