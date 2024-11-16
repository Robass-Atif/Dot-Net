using System.ComponentModel.DataAnnotations;
namespace E_commerce.Models
{
    public class Category
    {

        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }

        public  List<Product> products { get; set; }

    }
}
