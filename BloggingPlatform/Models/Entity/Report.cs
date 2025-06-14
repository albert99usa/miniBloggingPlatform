using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BloggingPlatform.Models.Entity
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("Blog")]
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }

        [Required]
        [EmailAddress]
        public string AuthorEmail { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public DateTime ReportedAt { get; set; } = DateTime.Now;
    }

}
