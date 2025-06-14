using Microsoft.AspNetCore.Identity;

namespace BloggingPlatform.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public UserRole Role { get; set; }
    }
}
