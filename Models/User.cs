using Microsoft.AspNetCore.Identity;

namespace WaterlooBites.Models
{
    public class User : IdentityUser
    {
        // Add any custom properties for the user
        public bool IsAdmin { get; set; }  // Add a property for admin check
    }

}
