using System.ComponentModel.DataAnnotations;


namespace E_commerce.Models
{
    public class Product
    {

        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string product_price { get; set; }
        public string product_image { get; set; }

        public int category_id { get; set; }
        public Category Category { get; set; }
    }
}
