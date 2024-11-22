using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_commerce.Models
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }
        public int product_id { get; set; }
        public int customer_id { get; set; }
        public int cart_quantity { get; set; }
        public int cart_status { get; set;  }

        [ForeignKey("product_id")]
        public Product Product { get; set; }
        [ForeignKey("customer_id")]
        public Customer Customer { get; set; }



    }
}
