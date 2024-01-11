using System.ComponentModel.DataAnnotations;
namespace Foodordering.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage ="Email is Required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage ="Email is not valid")]
        public string? Email { get; set; }

        
        [Required(ErrorMessage ="Password is Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$",ErrorMessage ="Password is not valid")]
        public string? Password { get; set; }
    }
}