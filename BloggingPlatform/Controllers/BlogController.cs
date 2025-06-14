using Microsoft.AspNetCore.Mvc;
using BloggingPlatform.Models;
using BloggingPlatform.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BloggingPlatform.Controllers
{
    [Controller]
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BlogController(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IAuthorRepository authorRepository , IHostingEnvironment hostingEnvironment)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _authorRepository = authorRepository;
            _hostingEnvironment = hostingEnvironment;
    }

        protected int? GetAuthorId()
        {
            return HttpContext.Session.GetInt32("AuthorID");
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            string authorName = HttpContext.Session.GetString("AuthorName");
            string authorEmail = HttpContext.Session.GetString("AuthorEmail");

            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }

            ViewBag.AuthorId = authorId;
            ViewBag.AuthorName = authorName;
            ViewBag.AuthorEmail = authorEmail;
            var blogs = _blogRepository.GetAllBlogs(authorId).ToList();
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = categories;
            return View("Index", blogs);
        }

        public IActionResult FilterByCategory(List<int> categoryIds)
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            string authorName = HttpContext.Session.GetString("AuthorName");
            string authorEmail = HttpContext.Session.GetString("AuthorEmail");

            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }

            ViewBag.AuthorId = authorId;
            ViewBag.AuthorName = authorName;
            ViewBag.AuthorEmail = authorEmail;

            var blogs = categoryIds.Any()
                ? _blogRepository.GetBlogsByCategories(categoryIds, authorId)
                : _blogRepository.GetAllBlogs(authorId);

            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategories = categoryIds;

            return View("Index", blogs);
        }


        [HttpGet("Blog/Blog/{id}")]
        public IActionResult BlogDetails(Guid id)
        {
            var blog = _blogRepository.GetBlogById(id);
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            string authorName = HttpContext.Session.GetString("AuthorName");
            string authorEmail = HttpContext.Session.GetString("AuthorEmail");

            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }
            if (blog == null)
            {
                return RedirectToAction("Error", new { message = "Blog not found." });
            }

            ViewBag.AuthorId = authorId;
            ViewBag.AuthorName = authorName;
            ViewBag.AuthorEmail = authorEmail;
            return View(blog);
        }

        [HttpGet("MyBlogs")]
        public IActionResult MyBlogs()
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            string authorName = HttpContext.Session.GetString("AuthorName");

            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }

            var blogs = _blogRepository.GetBlogsByAuthorId(authorId.Value);
            ViewBag.AuthorName = authorName;
            ViewBag.AuthorId = authorId;
            return View("MyBlogs", blogs);
        }

        [HttpGet("CreatePost")]
        public IActionResult CreatorPage(Guid? id)
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            string authorName = HttpContext.Session.GetString("AuthorName");

            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }

            Blog blog = new Blog();
            if (id.HasValue)
            {
                blog = _blogRepository.GetBlogById(id.Value);
                if (blog == null)
                {
                    return NotFound();
                }
            }

            ViewBag.AuthorName = authorName;
            ViewBag.AuthorId = authorId;

            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = categories;
            return View(blog);
        }

        [HttpPost("UploadImages")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
        {
            // Get the file from the POST request
            var theFile = HttpContext.Request.Form.Files.GetFile("file");

            // Get the server path, wwwroot
            string webRootPath = _hostingEnvironment.WebRootPath;

            // Building the path to the uploads directory (for images)
            var imageRoute = Path.Combine(webRootPath, "uploads/images");
            
            // Get the mime type
            var mimeType = theFile.ContentType;

            // Get File Extension
            string extension = Path.GetExtension(theFile.FileName);

            // Generate a random name for the file
            string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

            // Build the full path including the file name
            string link = Path.Combine(imageRoute, name);

            // Create directory if it does not exist.
            Directory.CreateDirectory(imageRoute);

            // Basic validation on image mime types and file extension
            string[] imageMimetypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };
            string[] imageExt = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            try
            {
                if (imageMimetypes.Contains(mimeType) && imageExt.Contains(extension))
                {
                    // Save the image to the server
                    using (var fileStream = new FileStream(link, FileMode.Create))
                    {
                        await theFile.CopyToAsync(fileStream);
                    }

                    // Return the file path as json
                    var fileUrl = new Hashtable();
                    fileUrl.Add("link", $"/uploads/images/{name}");

                    return Json(fileUrl);
                }

                throw new ArgumentException("The file is not a valid image");
            }
            catch (ArgumentException ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost("UploadFiles")]
        [Produces("application/json")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            // Get the file from the POST request
            var theFile = HttpContext.Request.Form.Files.GetFile("file");

            if (theFile == null)
            {
                return Json(new { error = "No file uploaded" });
            }

            // Get the server path (wwwroot)
            string webRootPath = _hostingEnvironment.WebRootPath;

            // Path to the uploads directory
            var fileRoute = Path.Combine(webRootPath, "uploads/files");

            // Get the mime type and file extension
            var mimeType = theFile.ContentType;
            var extension = Path.GetExtension(theFile.FileName);

            // Generate a random name for the file
            string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

            // Build the full path, including the file name
            string link = Path.Combine(fileRoute, name);

            // Create directory if it does not exist
            Directory.CreateDirectory(fileRoute);

            // Validate file types (allowed types)
            string[] allowedMimetypes = { "text/plain", "application/pdf", "application/msword", "application/json", "text/html" };
            string[] allowedExtensions = { ".txt", ".pdf", ".doc", ".json", ".html" };

            try
            {
                if (Array.IndexOf(allowedMimetypes, mimeType) >= 0 && Array.IndexOf(allowedExtensions, extension) >= 0)
                {
                    // Save the file
                    using (var stream = new FileStream(link, FileMode.Create))
                    {
                        await theFile.CopyToAsync(stream);
                    }

                    // Return the file path as JSON
                    return Json(new { link = "/uploads/" + name });
                }
                else
                {
                    throw new ArgumentException("Invalid file type.");
                }
            }
            catch (ArgumentException ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost("UploadVideos")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            var theFile = HttpContext.Request.Form.Files.GetFile("file");

            // Get the root path (wwwroot)
            string webRootPath = _hostingEnvironment.WebRootPath;

            // Directory where the video will be uploaded
            var fileRoute = Path.Combine(webRootPath, "uploads/videos");

            // Get mime type and extension
            var mimeType = theFile.ContentType;
            string extension = Path.GetExtension(theFile.FileName);

            // Generate a unique file name
            string fileName = Guid.NewGuid().ToString().Substring(0, 8) + extension;
            string fullPath = Path.Combine(fileRoute, fileName);

            // Supported video types
            string[] videoMimetypes = { "video/mp4", "video/webm", "video/ogg" };
            string[] videoExt = { ".mp4", ".webm", ".ogg" };

            // Validate and process the video
            if (Array.IndexOf(videoMimetypes, mimeType) >= 0 && Array.IndexOf(videoExt, extension) >= 0)
            {
                try
                {
                    // Create the directory if it does not exist
                    if (!Directory.Exists(fileRoute))
                    {
                        Directory.CreateDirectory(fileRoute);
                    }

                    // Save the video to the server
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await theFile.CopyToAsync(fileStream);
                    }

                    // Return the video link as JSON
                    var videoLink = new Hashtable();
                    videoLink.Add("link", $"/uploads/{fileName}");

                    return Json(videoLink);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            return BadRequest(new { message = "Invalid video format. Only MP4, WebM, and OGG are supported." });
        }

        [HttpPost("CreatePost")]
        public IActionResult CreatePost(Blog model)
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }

            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.AuthorId = authorId.Value;
                model.CreatedAt = DateTime.Now;
                _blogRepository.CreateBlog(model);
            }
            else
            {
                var existingBlog = _blogRepository.GetBlogById(model.Id);
                if (existingBlog == null)
                {
                    return NotFound();
                }
                existingBlog.Title = model.Title;
                existingBlog.Content = model.Content;
                existingBlog.AuthorId = authorId.Value;
                existingBlog.CategoryId = model.CategoryId;
                existingBlog.UpdatedAt = DateTime.Now;
                _blogRepository.UpdateBlog(existingBlog);
            }

            _blogRepository.Save();
            return RedirectToAction("MyBlogs");
        }

        [HttpGet("Delete/{id}")]
        public IActionResult DeletePost(Guid id)
        {
            Blog blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost("Blog/Delete/{id}")]
        public IActionResult DeletePostConfirmed(Guid id)
        {
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

        [HttpPost("Blog/Like/{id}")]
        public IActionResult LikePost(Guid id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }
            var authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }

            bool isLiked = _blogRepository.IsBlogLikedByAuthor(authorId.Value, id);
            if (isLiked)
            {
                TempData["ErrorMessage"] = "You have already liked this blog.";
                return RedirectToAction("LikedBlogs");
            }

            _blogRepository.LikeBlog(authorId.Value, id);
            blog.Likes += 1;
            _blogRepository.UpdateBlog(blog);
            _blogRepository.Save();
            return RedirectToAction("LikedBlogs");
        }

        [HttpPost("Blog/Comment/{id}")]
        public IActionResult CommentOnPost(Guid id, Comment comment)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return RedirectToAction("Error", new { message = "Blog not found." });
            }

            comment.BlogId = id;
            blog.Comments.Add(comment);
            _commentRepository.AddComment(comment);
            _commentRepository.Save();
            _blogRepository.UpdateBlog(blog);
            _blogRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpGet("Blog/LikedBlogs")]
        public IActionResult LikedBlogs()
        {
            var authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                return RedirectToAction("Login", "Author");
            }

            var likedBlogs = _blogRepository.GetLikedBlogsByAuthorId(authorId.Value);
            return View(likedBlogs);
        }

        [HttpPost]
        public IActionResult RemoveLike(Guid id)
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to perform this action.";
                return RedirectToAction("LikedBlogs");
            }

            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                TempData["ErrorMessage"] = "Blog not found.";
                return RedirectToAction("LikedBlogs");
            }

            var authorBlogLike = _blogRepository.GetAuthorBlogLike(authorId.Value, id);
            if (authorBlogLike != null)
            {
                _blogRepository.RemoveAuthorBlogLike(authorBlogLike);
                blog.Likes--;
                _blogRepository.UpdateBlog(blog);
                _blogRepository.Save();
            }

            return RedirectToAction("LikedBlogs");
        }

        [HttpPost]
        public IActionResult ReportBlog(Report report)
        {
            var blog = _blogRepository.GetBlogById(report.BlogId);
            if (blog == null)
            {
                return NotFound();
            }
            var authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }
            
            blog.ReportCount += 1;
            _blogRepository.AddReport(report);
            _blogRepository.UpdateBlog(blog);
            _blogRepository.Save();
            return RedirectToAction("Index");  
        }

        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
