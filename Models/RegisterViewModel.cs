using System.ComponentModel.DataAnnotations;
using Foodordering.Common;

namespace Foodordering.Models
{
    public class RegisterViewModel
    {
        [Required]
       [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Address { get; set; }

        [Required,EmailAddress]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [ValidEmailDomain(allowedDomain:"gmail.com",ErrorMessage ="Email Domain must be gmail.com")]
        public string? Email { get; set; }
        
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string? Password { get; set; }

    }
}