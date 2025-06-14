using BloggingPlatform.Models;
using BloggingPlatform.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorRepository _authorRepository;

        public AdminController(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IAuthorRepository authorRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _authorRepository = authorRepository;
        }
        public IActionResult Index()
        {
            var blogs = _blogRepository.GetReportedBlogs();
            return View(blogs);
        }
        
        public IActionResult DeletePost(Guid id) {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound(); // Or handle the error as appropriate
            }
            var comments = _commentRepository.GetCommentsByBlogId(id);
            foreach (var comment in comments)
            {
                _commentRepository.DeleteComment(comment.Id);
            }
            var reports = _blogRepository.GetReportsByBlogId(id);
            foreach (var report in reports)
            {
                _blogRepository.DeleteReport(report.ReportId);
            }
            var author = _authorRepository.GetAuthorById(blog.AuthorId);
            if (author != null)
            {
                author.Blogs.Remove(blog);
                _authorRepository.Update(author); 
            }
            var likes = _blogRepository.GetAllLikesOFBlog(id);
            foreach (var lik in likes)
            {
                _blogRepository.RemoveAuthorBlogLike(lik);
            }
            _blogRepository.DeleteBlog(id);
            _blogRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
