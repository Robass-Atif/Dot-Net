using System.ComponentModel.DataAnnotations;
namespace E_commerce.Models
{
    public class Feedback
    {
        [Key]
        public int feedback_id { get; set; }
        public string username { get; set; }

        public string user_message { get; set; }
    }
}
