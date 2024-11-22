using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

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
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			return View(MyContext.Customers.ToList());
        }

        public IActionResult EditCustomer(int id)
        {
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
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
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			return View(MyContext.Customers.Find(id));
		}
        public IActionResult DeleteCustomer(int id)
		{
			var customer = MyContext.Customers.Find(id);
			MyContext.Customers.Remove(customer);
			MyContext.SaveChanges();
			return RedirectToAction("Customer");
		}
        public IActionResult fetchCategory()
        {
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			return View(MyContext.Categories.ToList());
        }   
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            MyContext.Categories.Add(category);
            MyContext.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult EditCategory(int id)
        {
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			var category = MyContext.Categories.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            MyContext.Categories.Update(category);
            MyContext.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult ConfirmDeleteCategory(int id)
        {
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			return View(MyContext.Categories.Find(id));
        }
        public IActionResult DeleteCategory(int id)
        {
            var category = MyContext.Categories.Find(id);
            MyContext.Categories.Remove(category);
            MyContext.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult fetchProducts()
        {
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			return View(MyContext.Products.ToList());
        }
        public IActionResult AddProduct()
        {
			var result = HttpContext.Session.GetString("AdminEmail");
			if (result == null)
			{
				return RedirectToAction("Login");
			}
			List<Category> Categories = MyContext.Categories.ToList();
            ViewData["Categories"] = Categories;
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile product_image)
        {
                string ImagePath=Path.GetFileName(product_image.FileName);
                string filePath = Path.Combine(_env.WebRootPath, "productImages",ImagePath);

            // Save the uploaded file to the server
            var fs = new FileStream(filePath, FileMode.Create);
                
                    product_image.CopyTo(fs);
                

                // Update the product's image path with the new file name
                product.product_image = ImagePath;
            

            // Update the product record in the database
            MyContext.Products.Add(product);
            MyContext.SaveChanges();

            return RedirectToAction("fetchProducts");
        }
        public IActionResult EditProduct(int id)
        {
            ViewBag.Categories = MyContext.Categories.ToList();
            var product = MyContext.Products.Find(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult EditProduct(Product product, IFormFile product_image)
        {
            // Check if a new image has been uploaded
            if (product_image != null && product_image.Length > 0)
            {
                string filePath = Path.Combine(_env.WebRootPath, "productImages", product_image.FileName);

                // Save the uploaded file to the server
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    product_image.CopyTo(fs);
                }

                // Update the product's image path with the new file name
                product.product_image = product_image.FileName;
            }

            // Update the product record in the database
            MyContext.Products.Update(product);
            MyContext.SaveChanges();

            return RedirectToAction("fetchProducts");
        }
        public IActionResult ConfirmDeleteProduct(int id)
        {
            return View(MyContext.Products.Find(id));
        }
        public IActionResult DeleteProduct(int id) {
            
            var product = MyContext.Products.Find(id);
            MyContext.Products.Remove(product);
            MyContext.SaveChanges();
            return RedirectToAction("fetchProducts");
        }
        public IActionResult DetailsProduct(int id)
		{
            var product = MyContext.Products.Include(p => p.Category).FirstOrDefault(p => p.product_id == id);

            return View(product);
		}

        public IActionResult FeedBackAdmin()
        {
            
            return View(MyContext.Feedbacks.ToList());
            
        }
        public IActionResult ConfirmDeleteFeedback(int id)
        {
            return View(MyContext.Feedbacks.Find(id));
        }
        public IActionResult DeleteFeedback(int id)
        {
			var feedback = MyContext.Feedbacks.Find(id);

			MyContext.Feedbacks.Remove(feedback);
            MyContext.SaveChanges();
            return RedirectToAction("FeedBackAdmin");
        }
        public IActionResult FetchCart()
        {
            return View(MyContext.Carts.ToList());
        }
    }
}
