﻿using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
            List<Product> product = MyContext.Products.ToList();
            ViewData["Product"] = product;
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
        public IActionResult FeedBack()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FeedBack(Feedback feedback)
        {
            MyContext.Feedbacks.Add(feedback);
            MyContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult AllProducts()
        {
            ViewBag.checkSession = HttpContext.Session.GetInt32("customer_id");
            return View(MyContext.Products.ToList());

        }
        public IActionResult AddCart(int id)
        {
            ViewBag.checkSession = HttpContext.Session.GetInt32("customer_id");

            Product product = MyContext.Products.FirstOrDefault(p => p.product_id == id);
            return View(product);
        }
        // POST: Add product to cart
        [HttpPost]
        public IActionResult AddCart(int product_id, Cart cart)
        {
            int? customer_id = HttpContext.Session.GetInt32("customer_id");

            if (customer_id.HasValue)
            {
                // Log the product_id for debugging (optional)
                Console.WriteLine("Product ID: " + product_id);

                // Initialize the cart object with necessary data
                cart.product_id = product_id;
                cart.customer_id = customer_id.Value;
                cart.cart_quantity = 1; // Default quantity to 1
                cart.cart_status = 0;   // Assuming 0 means it's in the cart and not purchased yet

                // Add the cart item to the database
                MyContext.Carts.Add(cart);

               
                    // Save changes to the database
                    MyContext.SaveChanges();

                    // Set a success message in TempData for one-time message display
                    TempData["Message"] = "Product added to cart successfully!";
                
             
                Product product = MyContext.Products.FirstOrDefault(p => p.product_id == product_id);
              
                ViewBag.checkSession = HttpContext.Session.GetInt32("customer_id");
                return View(product);  // Returning the same view with the product data
           
            }

            return RedirectToAction("Login");
        }

        public IActionResult AllCart()
        {
            int? customer_id = HttpContext.Session.GetInt32("customer_id");

            if (customer_id.HasValue)

            {
                ViewBag.checkSession = HttpContext.Session.GetInt32("customer_id");

                List<Cart> carts = MyContext.Carts.Include(c => c.Product).Where(c => c.customer_id == customer_id).ToList();
                return View(carts);
            }

            return RedirectToAction("Login");
        }

        public IActionResult DeleteCart(int id)
        {
            Cart cart = MyContext.Carts.FirstOrDefault(c => c.cart_id == id);
            MyContext.Carts.Remove(cart);
            MyContext.SaveChanges();
            return RedirectToAction("AllCart");
        }



    }
}
