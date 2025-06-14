using System.ComponentModel.DataAnnotations;

namespace BloggingPlatform.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
    
}
