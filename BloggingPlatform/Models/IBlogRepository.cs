using BloggingPlatform.Models.Entity;

namespace BloggingPlatform.Models
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> GetAllBlogs(int? authorId);
        Blog GetBlogById(Guid id);
        IEnumerable<Blog> GetBlogsByAuthorId(int authorId);
        void CreateBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(Guid id);
        void Save();
        void LikeBlog(int authorId, Guid blogId);
        IEnumerable<Blog> GetLikedBlogsByAuthorId(int authorId);
        IEnumerable<Blog> GetBlogsByCategoryId(int? categoryId, int? authorId);
        bool IsBlogLikedByAuthor(int authorId, Guid blogId);
        AuthorBlogLike GetAuthorBlogLike(int authorId, Guid blogId);
        void RemoveAuthorBlogLike(AuthorBlogLike authorBlogLike);
        void AddReport(Report report);
        public IEnumerable<Blog> GetReportedBlogs();
        public IEnumerable<Report> GetReportsByBlogId(Guid id);
        void DeleteReport(int reportId);
        public IEnumerable<AuthorBlogLike> GetAllLikesOFBlog(Guid id);
        public IEnumerable<Blog> GetBlogsByCategories(List<int> categoryIds, int? authorId);
    }
}
