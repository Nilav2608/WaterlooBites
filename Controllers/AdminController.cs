using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WaterlooBites.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize] // This ensures that only authenticated users can access the controller
        public IActionResult Index()
        {
            // Admin is redirected to the dashboard directly after login, this is an optional view.
            return RedirectToAction(nameof(AdminDashboard));
        }

        // GET: Admin/Login
        [AllowAnonymous] // Allow unauthenticated access
        public IActionResult AdminLogin()
        {
            return View();
        }

        // GET: Admin/AdminDashboard
        [Authorize] // Ensuring that only authenticated users can access the dashboard
        public IActionResult AdminDashboard()
        {
            return View(); // This will render the AdminDashboard.cshtml view
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(string username, string password, string? returnUrl = null)
        {
            if (username == "ConestogaCollege.123" && password == "ConestogaCollege.123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Redirect to the return URL or AdminDashboard
                return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction(nameof(AdminDashboard), "Admin");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // POST: Admin/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user and redirect to the login page
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AdminLogin));
        }

        // GET: Admin/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
