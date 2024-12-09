using Microsoft.EntityFrameworkCore;
using WaterlooBites.Data;

namespace WaterlooBites.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, IWebHostEnvironment hostingEnvironment)
        {
            using (
                var context = new WaterlooBitesContext(
                serviceProvider.GetRequiredService<DbContextOptions<WaterlooBitesContext>>()))
            {
                context.Database.EnsureCreated();

                // Check if there are already records in the database
                if (context.Rating.Any())
                {
                    return;   // DB has been seeded
                }

                var ratings = new List<Rating>
            {
                new Rating
                {
                    RestaurantName = "The Food Place",
                    FoodName = "Pizza",
                    Score = 5,
                    price = 15,
                    Date = DateTime.Now,
                    Image = File.ReadAllBytes("wwwroot/images/burger.jpg")
                },
                new Rating
                {
                    RestaurantName = "Sushi House",
                    FoodName = "Sushi",
                    Score = 4,
                    price = 20,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "sushi.jpg")
                },
                new Rating
                {
                    RestaurantName = "Burger Joint",
                    FoodName = "Cheeseburger",
                    Score = 3,
                    price = 10,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "burger.jpg")
                },
                new Rating
                {
                    RestaurantName = "Pasta Paradise",
                    FoodName = "Spaghetti",
                    Score = 5,
                    price = 18,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "spaghetti.jpg")
                },
                new Rating
                {
                    RestaurantName = "The Curry Corner",
                    FoodName = "Chicken Curry",
                    Score = 4,
                    price = 14,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "chickencurry.jpg")
                },
                new Rating
                {
                    RestaurantName = "Taco Town",
                    FoodName = "Tacos",
                    Score = 4,
                    price = 12,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "Tacos.jpg")
                },
                new Rating
                {
                    RestaurantName = "Salad Stop",
                    FoodName = "Caesar Salad",
                    Score = 3,
                    price = 8,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "CaesarSalad.jpg")
                },
                new Rating
                {
                    RestaurantName = "Pancake House",
                    FoodName = "Pancakes",
                    Score = 5,
                    price = 7,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "Pancakes.jpg")
                },
                new Rating
                {
                    RestaurantName = "Steakhouse",
                    FoodName = "Steak",
                    Score = 4,
                    price = 25,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "steak.jpg")
                },
                new Rating
                {
                    RestaurantName = "Noodle Bar",
                    FoodName = "Ramen",
                    Score = 4,
                    price = 12,
                    Date = DateTime.Now,
                    Image = GetImageByteArray(hostingEnvironment, "ramen.jpg")
                }
            };



                context.Rating.AddRange(ratings);
                context.SaveChanges();
            }
        }
        

        // Helper method to read image files into byte arrays
        // Convert image to byte array (placeholder method, replace with actual implementation)
        private static byte[] GetImageByteArray(IWebHostEnvironment hostingEnvironment, string imageName)
        {


            var imagePath = Path.Combine(hostingEnvironment.WebRootPath, "images", imageName);

            // Log the path for debugging
            Console.WriteLine($"Image path: {imagePath}");

            if (File.Exists(imagePath))
            {
                Console.WriteLine("File exists!");
                return File.ReadAllBytes(imagePath);
            }

            Console.WriteLine("File does not exist or cannot be accessed.");
            return null!;
        }
    }
}
