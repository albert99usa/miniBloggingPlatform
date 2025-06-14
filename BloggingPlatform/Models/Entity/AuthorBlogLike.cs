namespace BloggingPlatform.Models.Entity
{
    public class AuthorBlogLike
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }

}
