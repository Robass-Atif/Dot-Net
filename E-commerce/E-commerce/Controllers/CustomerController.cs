using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class CustomerController : Controller

    {
        private MyContext MyContext { get; set; }
        private IWebHostEnvironment _env;
        public CustomerController(MyContext myContext, IWebHostEnvironment _env) {
        this.MyContext = myContext;
            this._env = _env;
        }
        public IActionResult Index()
        {
            List<Category> category=MyContext.Categories.ToList();
            ViewData["Category"]=category;
            ViewBag.checkSession = HttpContext.Session.GetInt32("customer_id");
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string customerEmail, string customerPassword)
        {
            Customer customer = MyContext.Customers.FirstOrDefault(c => c.customer_email == customerEmail && c.customer_password == customerPassword);
            if (customer == null)
            {
                ViewBag.Message = "Invalid email or password";
                return View();
            }
            HttpContext.Session.SetInt32("customer_id", customer.customer_id);
            return RedirectToAction("Index");
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Customer customer)
        {
            // Check if the customer already exists
            Customer existingCustomer = MyContext.Customers.FirstOrDefault(c => c.customer_email == customer.customer_email);
            if (existingCustomer != null)
            {
                // Customer already exists
                ViewBag.Message = "Customer already exists";
                return View();
            }
            // Add the customer to the database
            MyContext.Customers.Add(customer);
            MyContext.SaveChanges();

            HttpContext.Session.SetInt32("customer_id", customer.customer_id);
            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_id");
            return RedirectToAction("Index");
        }
        public IActionResult Profile()
        {
            Customer customer = MyContext.Customers.FirstOrDefault(c => c.customer_id == HttpContext.Session.GetInt32("customer_id"));

            return View(customer);
        }
        [HttpPost]
        public IActionResult Profile(Customer customer,IFormFile customer_image)
        {
            var filepath= Path.Combine(_env.WebRootPath, "customerImages", customer_image.FileName);
            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                customer_image.CopyTo(fileStream);
            }
            customer.customer_image = customer_image.FileName;
            MyContext.Customers.Update(customer);
            MyContext.SaveChanges();
            return RedirectToAction("Profile");
        

           


        }
    }
}
