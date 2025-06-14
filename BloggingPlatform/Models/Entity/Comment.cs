using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingPlatform.Models.Entity
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Blog")]
        public Guid BlogId { get; set; }  // Foreign key to the blog

        [Required]
        public string Author { get; set; }  // Name of the commenter

        [Required]
        [EmailAddress]
        public string Email { get; set; }  // Email of the commenter

        [Required]
        public string Content { get; set; }  // The comment content

        public bool IsApproved { get; set; }  // Indicates if the comment is approved
        public Blog Blog { get; set; }  // Navigation property to the blog
    }
}
