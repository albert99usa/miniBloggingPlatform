using BloggingPlatform.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatform.Models
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BloggingPlatformContext _context;

        public BlogRepository(BloggingPlatformContext context)
        {
            this._context = context;
        }

        public IEnumerable<Blog> GetAllBlogs(int? authorId)
        {
            var blog = new List<Blog>();
            //blog = _context.Blogs.Include(b => b.Comments).Include(b => b.Author).Include(b => b.Category).Where(b => b.AuthorId != authorId).ToList();
            blog = _context.Blogs.Include(b => b.Comments).Include(b => b.Author).Include(b => b.Category).Where(b => b.AuthorId == authorId).ToList();
            return blog;
        }

        public Blog GetBlogById(Guid id)
        {
            return _context.Blogs.Include(b => b.Comments).Include(b => b.Author).Include(b => b.Category).FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Blog> GetBlogsByCategoryId(int? categoryId, int? authorId)
        {
            return _context.Blogs
                           .Include(b => b.Comments)
                           .Include(b => b.Author)
                           .Include(b => b.Category)
                           .Where(b => b.AuthorId != authorId && b.CategoryId == categoryId)
                           .ToList();  
        }
        public IEnumerable<Blog> GetBlogsByCategories(List<int> categoryIds, int? authorId)
        {
            return _context.Blogs
                 //.Where(b => b.AuthorId != authorId && categoryIds.Contains(b.CategoryId))
                .Where(b => b.AuthorId == authorId && categoryIds.Contains(b.CategoryId))
                .Include(b => b.Author)
                .Include(b => b.Comments)
                .Include(b => b.Category)
                .ToList();
        }

        public IEnumerable<Blog> GetBlogsByAuthorId(int authorId)
        {
            return _context.Blogs.Include(b => b.Comments).Where(b => b.AuthorId == authorId).ToList();
        }

        public void CreateBlog(Blog blog)
        {
            blog.Id = Guid.NewGuid();
            _context.Blogs.Add(blog);
        }

        public void UpdateBlog(Blog blog)
        {
            _context.Blogs.Update(blog);
        }

        public void DeleteBlog(Guid id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        
        // Like Related Oprations

        public void LikeBlog(int authorId, Guid blogId)
        {
            var likedBlog = new AuthorBlogLike
            {
                AuthorId = authorId,
                BlogId = blogId
            };

            _context.AuthorBlogLike.Add(likedBlog);
            _context.SaveChanges();
        }

        public IEnumerable<Blog> GetLikedBlogsByAuthorId(int authorId)
        {
            return _context.AuthorBlogLike
                           .Where(like => like.AuthorId == authorId)
                           .Select(like => like.Blog)
                           .ToList();
        }

        public bool IsBlogLikedByAuthor(int authorId, Guid blogId)
        {
            return _context.AuthorBlogLike
                           .Any(like => like.AuthorId == authorId && like.BlogId == blogId);
        }

        public AuthorBlogLike GetAuthorBlogLike(int authorId, Guid blogId)
        {
            return _context.AuthorBlogLike.FirstOrDefault(x => x.AuthorId == authorId && x.BlogId == blogId);
        }

        public void RemoveAuthorBlogLike(AuthorBlogLike authorBlogLike)
        {
            _context.AuthorBlogLike.Remove(authorBlogLike);
        }
        public IEnumerable<AuthorBlogLike> GetAllLikesOFBlog(Guid id)
        {
            return _context.AuthorBlogLike.Where(like => like.BlogId == id);
        }
        

        // CRUD on Reports
        public void AddReport(Report report)
        {
            _context.Reports.Add(report);
            _context.SaveChanges();
        }
        public void DeleteReport(int id)
        {
            var report = _context.Reports.FirstOrDefault(r => r.ReportId == id);
            if (report != null)
            {
                _context.Reports.Remove(report);
            }
        }
        public IEnumerable<Blog> GetReportedBlogs()
        {
            return _context.Blogs.Include(b => b.Author).Include(b => b.Reports).Where(b => b.Reports.Count() >= 1).ToList();
        }
        public IEnumerable<Report> GetReportsByBlogId(Guid id)
        {
            return _context.Reports.Where(r => r.BlogId == id);
        }
       
    }
}
