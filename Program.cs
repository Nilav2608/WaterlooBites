using Microsoft.EntityFrameworkCore;
using WaterlooBites.Data;
using WaterlooBites.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WaterlooBitesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WaterlooBitesContext") ?? throw new InvalidOperationException("Connection string 'WaterlooBitesContext' not found.")));

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Set to your login path
        options.AccessDeniedPath = "/Account/AccessDenied";  // Set to your access denied path
    });

// Add authorization services (if required)
builder.Services.AddAuthorization();

// Add MVC or Razor Pages services
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<WaterlooBitesContext>();
        SeedData.Initialize(services, services.GetRequiredService<IWebHostEnvironment>());
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while seeding the database.");
        Console.WriteLine(ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add the authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Method to seed data into the database
//void SeedDatabase(WebApplication app)
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var context = scope.ServiceProvider.GetRequiredService<WaterlooBitesContext>();
//        context.Database.Migrate();  // Ensure the database is up-to-date with migrations

//        // Check if there are existing ratings to avoid duplication
//        if (!context.Rating.Any())
//        {
//            // Directory where the images are stored in the wwwroot folder
//            var imagesDirectory = Path.Combine(app.Environment.WebRootPath, "images");

//            // Add predefined ratings with images stored as byte arrays
//            var ratings = new List<Rating>
//            {
//                new Rating
//                {
//                    RestaurantName = "The Food Place",
//                    FoodName = "Pizza",
//                    Score = 5,
//                    price = 15,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "pizza.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Sushi House",
//                    FoodName = "Sushi",
//                    Score = 4,
//                    price = 20,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "sushi.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Burger Joint",
//                    FoodName = "Cheeseburger",
//                    Score = 3,
//                    price = 10,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "cheeseburger.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Pasta Paradise",
//                    FoodName = "Spaghetti",
//                    Score = 5,
//                    price = 18,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "spaghetti.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "The Curry Corner",
//                    FoodName = "Chicken Curry",
//                    Score = 4,
//                    price = 14,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "chickencurry.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Taco Town",
//                    FoodName = "Tacos",
//                    Score = 4,
//                    price = 12,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "tacos.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Salad Stop",
//                    FoodName = "Caesar Salad",
//                    Score = 3,
//                    price = 8,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "caesarsalad.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Pancake House",
//                    FoodName = "Pancakes",
//                    Score = 5,
//                    price = 7,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "pancakes.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Steakhouse",
//                    FoodName = "Steak",
//                    Score = 4,
//                    price = 25,
//                    Date = DateTime.Now,
//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "steak.jpg"))
//                },
//                new Rating
//                {
//                    RestaurantName = "Noodle Bar",
//                    FoodName = "Ramen",
//                    Score = 4,
//                    price = 12,
//                    Date = DateTime.Now,

//                    Image = File.ReadAllBytes(Path.Combine(imagesDirectory, "ramen.png"))
//                }
//            };

//            context.Rating.AddRange(ratings);
//            context.SaveChanges();  // Save changes to the database

//            // Optional: Log the seeding process
//            Console.WriteLine("Predefined reviews with images have been added to the database.");
//        }
//        else
//        {
//            // Optional: Log if data already exists
//            Console.WriteLine("Ratings already exist in the database.");
//        }
//    }
//}
// Method to seed data into the database



// <img src="data:image/jpeg;base64,@Convert.ToBase64String(item.Image)" alt="Food Image" width="100" height="100" />