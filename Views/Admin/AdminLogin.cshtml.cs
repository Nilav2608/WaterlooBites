using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using WaterlooBites.Models;  // Your ApplicationUser model
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WaterlooBites.Views.Admin
{
    public class AdminLoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        // Constructor to inject services
        public AdminLoginModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Check if the admin exists in the database
                var admin = await _userManager.FindByNameAsync(Username!);

                // Validate the password for the admin user
                if (admin != null && await _userManager.CheckPasswordAsync(admin, Password!))
                {
                    // Set the session or authentication cookie for the admin to stay logged in
                    await _signInManager.SignInAsync(admin, isPersistent: false);

                    // Redirect to Admin Dashboard
                    return RedirectToPage("/AdminDashboard");
                }
                else
                {
                    // Invalid login attempt
                    ErrorMessage = "Invalid username or password!";
                    return Page();
                }
            }

            return Page();
        }
    }
}
