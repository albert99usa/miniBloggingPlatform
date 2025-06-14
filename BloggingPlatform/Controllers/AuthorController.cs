using BloggingPlatform.Models;
using BloggingPlatform.Models.Entity;
using BloggingPlatform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;

namespace BloggingPlatform.Controllers
{
    [Route("authors")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepo;

        public AuthorController(IAuthorRepository authorRepo)
        {
            this._authorRepo = authorRepo;
        }
        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            var model = _authorRepo.GetAllAuthors();
            return View(model);
        }
        [HttpGet("details")]
        public IActionResult Details()
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }
            Author author = _authorRepo.GetAuthorById((int)authorId);
            if(author == null)
            {
                Response.StatusCode = 404;
                return View("authorNotFound" , authorId);
            }
            return View(author);
        }

        [HttpGet("add")]
        public IActionResult Create()
        {
            ViewBag.RoleList = new SelectList(Enum.GetValues(typeof(UserRole)));
            return View();
        }
        [HttpPost("add")]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _authorRepo.GetAuthorByEmail(author.Email);
                if(existingUser == null)
                {
                    Author newAuthor = _authorRepo.Add(author);
                    newAuthor.Role = UserRole.Writer;
                    _authorRepo.save();
                    return RedirectToAction("details" , new {id = newAuthor.ID});
                }
                ModelState.AddModelError("", "Email Already in Use.");
            }
            return View();
        }

        [HttpGet("edit")]
        public IActionResult Edit(int id)
        {
            Author author = _authorRepo.GetAuthorById(id);
            return View(author);
        }
        [HttpPost("edit")]
        public IActionResult Edit(Author modifiedAuthor)
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }
            if (ModelState.IsValid)
            {
                Author author = _authorRepo.GetAuthorById(modifiedAuthor.ID);

                if (author == null)
                {
                    // Handle case when the author doesn't exist (e.g., return 404)
                    return NotFound();
                }

                // Update the author properties with the new data from modifiedAuthor
                author.FirstName = modifiedAuthor.FirstName;
                author.LastName = modifiedAuthor.LastName;
                author.Email = modifiedAuthor.Email;
                author.Password = modifiedAuthor.Password;
                author.Role = modifiedAuthor.Role;
                
                // Update the author in the repository
                _authorRepo.Update(author);
                _authorRepo.save();

                // Redirect to the Index view or another appropriate action after successful edit
                return RedirectToAction("Details");
            }

            // If the model state is invalid, return the same view with the modifiedAuthor object
            return View(modifiedAuthor);
        }

        [HttpGet("delete")]
        public IActionResult Delete(int id)
        {
            Author author = _authorRepo.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
        [HttpPost("delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            int? authorId = HttpContext.Session.GetInt32("AuthorID");
            if (authorId == null)
            {
                TempData["ErrorMessage"] = "You Need to Login first";
                return RedirectToAction("Login", "Author");
            }
            var author = _authorRepo.GetAuthorById(id);
            _authorRepo.Delete(author.ID);
            _authorRepo.save();
            return RedirectToAction("Index");
        }

        // login and signup controls
        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return RedirectToAction("Create");
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Writer", Text = "Writer" },
                new SelectListItem { Value = "Administrative", Text = "Administrative" }
            };
            return View("~/Views/Author/Login.cshtml");
        }

        [HttpPost("login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var author = _authorRepo.GetAuthorByEmail(model.Email);
                if (author == null)
                {
                    // User does not exist
                    ModelState.AddModelError("", "User does not exist with this email. Please create an account first.");
                }
                else
                {
                    if (author.Password != model.Password)
                    {
                        ModelState.AddModelError("", "Invalid password.");
                    }
                    else
                    {
                        var role = author.Role;
                        // Set session variables
                        HttpContext.Session.SetInt32("isLoggedIn", 1);
                        HttpContext.Session.SetString("UserRole", role.ToString());
                        HttpContext.Session.SetString("AuthorName", author.FullName);
                        HttpContext.Session.SetString("AuthorEmail", author.Email);
                        HttpContext.Session.SetInt32("AuthorID", author.ID);
                        // Redirect based on role
                        if (role == UserRole.Writer)
                        {
                            return RedirectToAction("Index", "Blog");
                        }
                        else if (role == UserRole.Administrator)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                }
            }

            return View("~/Views/Author/Login.cshtml", model);  // Return the view with the model for validation error display
        }


        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Author");
        }
    }
}
