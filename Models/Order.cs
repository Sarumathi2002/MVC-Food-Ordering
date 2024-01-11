using System.ComponentModel.DataAnnotations;
namespace Foodordering.Models
{
    public class Order
    {
        [Key]
        public int Id {get; set; }

        [Required]
        public string? RecipeId {get; set; }

        [Required]
        public string? RecipeName {get; set; }

        [Required]
        public string? UserId {get; set; }

        [Required(ErrorMessage ="Table Number is Required")]
        public string? Address {get; set; }

        [Required]
        public int Price {get; set; }

        [Required]
        public int Quantity {get; set; }

        [Required]
        public int TotalAmount {get; set; }
        
        public DateTime orderDate {get; set; }

    }
}
