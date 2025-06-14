using BloggingPlatform.Models.Entity;

namespace BloggingPlatform.Models
{
    public interface ICommentRepository
    {
        public void AddComment(Comment comment);
        public void Save();
        public IEnumerable<Comment> GetCommentsByBlogId(Guid id);
        public void DeleteComment(int id);
    }
}
