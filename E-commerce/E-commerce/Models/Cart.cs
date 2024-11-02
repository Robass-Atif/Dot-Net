using System.ComponentModel.DataAnnotations;
namespace E_commerce.Models
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }
        public int product_id { get; set; }
        public int customer_id { get; set; }
        public int cart_quantity { get; set; }
        public int cart_status { get; set; }
    }
}
