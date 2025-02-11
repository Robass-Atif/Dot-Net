﻿using System.ComponentModel.DataAnnotations;
namespace E_commerce.Models
{
    public class Customer
    {
        [Key]
      public int customer_id { get;set; }
        public string customer_name { get; set; }
        public string customer_email { get; set; }
        public string customer_password { get; set; }
        public string? customer_phone { get; set; }

        public string? customer_address { get; set; }
        public string? customer_image { get; set; }
        public string? customer_country { get; set; }
        public string? customer_city { get; set; }


    }
}
