using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace BloggingPlatform.Models.Entity
{
    public class Blog
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public int Likes { get; set; } = 0; 

        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }  
        public Category Category { get; set; } 
        public ICollection<Comment> Comments { get; set; }

        public int ReportCount { get; set; } = 0; 
        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public ICollection<AuthorBlogLike> LikedByAuthors { get; set; } = new List<AuthorBlogLike>();

        public Blog()
        {
            Comments = new List<Comment>(); 
        }
    }
}
