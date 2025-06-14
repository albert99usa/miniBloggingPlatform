using BloggingPlatform.Models.Entity;

namespace BloggingPlatform.Models
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BloggingPlatformContext _context;

        public CommentRepository(BloggingPlatformContext context)
        {
            this._context = context;
        }

        public IEnumerable<Comment> GetCommentsByBlogId(Guid id)
        {
            return _context.Comments.Where(c => c.BlogId == id);
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }
        public void DeleteComment(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
