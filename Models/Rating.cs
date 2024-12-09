using System.ComponentModel.DataAnnotations;

namespace WaterlooBites.Models
{
    public class Rating
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60,MinimumLength =3)]
        public string? RestaurantName { get; set; }

        [Required]
        [StringLength(60,MinimumLength =3)]
        public string? FoodName { get; set; }

        [RegularExpression(@"^\d{1,7}(\.\d{1,2})?$", ErrorMessage = "Please enter valid price")]
        public double price { get; set; }
        [Required]
        [Range(0,5,ErrorMessage ="review must in betwen 0 to 5")]
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public byte[]? Image { get; set; }


     
    }
}
