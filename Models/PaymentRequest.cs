using System.ComponentModel.DataAnnotations;
using System.Web;
namespace Foodordering.Models
{
    public class RazorPayOrder
    {
        public string? OrderId { get; set; }
        public string? RazorPayAPIKey { get; set; }
        public int Amount;
        public string? Currency { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [StringLength(50, MinimumLength = 3,ErrorMessage ="The string length is not applicable")]
        public string? Name { get; set; }

        [Required(ErrorMessage ="Email is Required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage ="Email is not valid")]
        public string? Email { get; set; }
       
        }





    public class PaymentRequest
    {
        [Required(ErrorMessage ="Name is required")]
        [StringLength(50, MinimumLength = 3,ErrorMessage ="The string length is not applicable")]
        public string? Name { get; set; }
        
         [Required(ErrorMessage ="Email is Required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage ="Email is not valid")]
        public string? Email { get; set; }
        [Required]
        public int Amount=1542;
    }
}
