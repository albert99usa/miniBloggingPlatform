using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingPlatform.Models.Entity
{
    public class Author : EntityBase
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public UserRole Role { get; set; } = UserRole.Writer;

        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<AuthorBlogLike> LikedBlogs { get; set; } = new List<AuthorBlogLike>();
        public Author()
        {
            Blogs = new List<Blog>();
        }
    }

    public enum UserRole : byte
    {
        Administrator,
        Writer
    }
}
