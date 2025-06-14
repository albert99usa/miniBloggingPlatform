using System.ComponentModel.DataAnnotations;

namespace BloggingPlatform.Models.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }  // Category name

        public string Slug { get; set; }  // URL-friendly category name

        public ICollection<Blog> Blogs { get; set; }  // Navigation to associated blogs

        public Category()
        {
            Blogs = new List<Blog>();
        }
    }
}
