using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_commerce.Controllers
{
    public class AdminController : Controller
    {
        private MyContext MyContext;
        private IWebHostEnvironment _env;

        // Constructor to initialize context and environment
        public AdminController(MyContext context, IWebHostEnvironment env)
        {
            MyContext = context;
            this._env = env;
        }

        // Index action: Checks if Admin is logged in
        public IActionResult Index()
        {
            var result = HttpContext.Session.GetString("AdminEmail");
            if (result == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // Login GET action to show login page
        public IActionResult Login()
        {
            return View();
        }

        // Login POST action to authenticate and log in admin
        [HttpPost]
        public IActionResult Login(string AdminEmail, string AdminPassword)
        {
            var raw = MyContext.Admins
                .Where(a => a.admin_email == AdminEmail && a.admin_password == AdminPassword)
                .FirstOrDefault();

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

        // Logout action to clear session and redirect to login
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }

        // Profile GET action to display admin profile
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

        // Profile POST action to update admin profile
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            MyContext.Admins.Update(admin);
            MyContext.SaveChanges();

            return RedirectToAction("Profile");
        }

        // ChangeImage action to upload a new profile image
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
        public IActionResult Customer() {
            return View(MyContext.Customers.ToList());
        }

        public IActionResult EditCustomer(int id)
        {
            var customer = MyContext.Customers.Find(id);
            return View(customer);
        }
        [HttpPost]
		public IActionResult EditCustomer(Customer customer, IFormFile customer_image)
		{
			// Check if a new image has been uploaded
			if (customer_image != null && customer_image.Length > 0)
			{
				
				string filePath = Path.Combine(_env.WebRootPath, "customerImages", customer_image.FileName);

				// Save the uploaded file to the server
				using (var fs = new FileStream(filePath, FileMode.Create))
				{
					customer_image.CopyTo(fs);
				}

				// Update the customer's image path with the new file name
				customer.customer_image = customer_image.FileName;
			}

			// Update the customer record in the database
			MyContext.Customers.Update(customer);
			MyContext.SaveChanges();

			return RedirectToAction("Customer");
		}

		public IActionResult ConfirmDelete(int id)
        {
            return View(MyContext.Customers.Find(id));
		}
        public IActionResult DeleteCustomer(int id)
		{
			var customer = MyContext.Customers.Find(id);
			MyContext.Customers.Remove(customer);
			MyContext.SaveChanges();
			return RedirectToAction("Customer");
		}
	}
}
