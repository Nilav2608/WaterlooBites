using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaterlooBites.Models;
using WaterlooBites.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WaterlooBites.Controllers
{
    public class RatingsController : Controller
    {
        private readonly WaterlooBitesContext _context;

        public RatingsController(WaterlooBitesContext context)
        {
            _context = context;
        }

        // GET: Ratings
        public IActionResult Index(string restaurantName, string foodName, string score)
        {
            // Start with all ratings from the database
            var ratings = _context.Rating.AsQueryable();

            // Apply filters if search criteria are provided
            if (!string.IsNullOrEmpty(restaurantName))
            {
                ratings = ratings.Where(r => r.RestaurantName!.Contains(restaurantName));
            }

            if (!string.IsNullOrEmpty(foodName))
            {
                ratings = ratings.Where(r => r.FoodName!.Contains(foodName));
            }

            if (!string.IsNullOrEmpty(score) && int.TryParse(score, out int scoreValue))
            {
                ratings = ratings.Where(r => r.Score == scoreValue);
            }

            // Return the filtered or all ratings to the view
            return View(ratings.ToList());
        }


        // GET: Ratings/Create
        public IActionResult Create()
        {
            return View(); // Return the view for creating a new rating
        }

        // POST: Ratings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantName, FoodName, Score, price, Date, Image")] Rating rating, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                
                if (Image != null && Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // Copy the uploaded image into the memory stream
                        await Image.CopyToAsync(memoryStream);

                        // Convert the image to a byte array
                        rating.Image = memoryStream.ToArray();
                    }
                }

                _context.Add(rating);  // Save the rating (including the image) to the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirect to the index view
            }
            return View(rating);  // If the model state is invalid, return to the same view
        }

        // GET: Ratings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If id is null, return NotFound
            }

            var rating = _context.Rating.Find(id); // Find the rating by id
            if (rating == null)
            {
                return NotFound(); // If no rating is found, return NotFound
            }

            // Convert Rating to RatingsViewModel
            var ratingViewModel = new RatingsViewModel
            {
                Id = rating.Id,
                RestaurantName = rating.RestaurantName,
                FoodName = rating.FoodName,
                Score = rating.Score,
                price = rating.price,
                Date = rating.Date
            };

            return View(ratingViewModel); // Pass the RatingsViewModel to the view
        }


        // POST: Ratings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id, RestaurantName, FoodName, Score, price, Date, Image")] Rating rating, IFormFile Image)
        {
            if (id != rating.Id)
            {
                return NotFound(); // If the id doesn't match, return NotFound
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null && Image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            // Copy the uploaded image into the memory stream
                             Image.CopyTo(memoryStream);

                            // Convert the image to a byte array
                            rating.Image = memoryStream.ToArray();
                        }
                    }
                    _context.Update(rating); // Update the rating in the database
                    _context.SaveChanges(); // Save changes
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Rating.Any(e => e.Id == rating.Id))
                    {
                        return NotFound(); // If the rating is no longer available, return NotFound
                    }
                    else
                    {
                        throw; // Otherwise, throw the exception to be caught
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to Index after successful edit
            }
            return View(rating); // If model state is invalid, return the same view
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var restaurant = await _context.Rating.FindAsync(id);
            if (restaurant?.Image != null)
            {
                return File(restaurant.Image, "image/jpg"); // Change "image/jpeg" based on your image type
            }
            return NotFound();
        }

        // GET: Ratings/Delete/5
        [Authorize(Roles = "Admin")] // Admins can access the delete action
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If id is null, return NotFound
            }

            var rating = _context.Rating.FirstOrDefault(m => m.Id == id); // Find the rating by id
            if (rating == null)
            {
                return NotFound(); // If no rating is found, return NotFound
            }

            return View(rating); // Return the Delete view with the found rating
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Admins can delete reviews
        public IActionResult DeleteConfirmed(int? id)
        {
            var rating = _context.Rating.Find(id); // Find the rating by id
            _context.Rating.Remove(rating!); // Remove the rating from the database
            _context.SaveChanges(); // Save changes to apply the deletion
            return RedirectToAction(nameof(Index)); // Redirect to the Index after deletion
        }
    }
}
