using BloggingPlatform.Models.Entity;

namespace BloggingPlatform.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BloggingPlatformContext _context;

        public CategoryRepository(BloggingPlatformContext context)
        {
            this._context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList(); // Fetch all categories
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id); // Fetch a category by ID
        }

        public void CreateCategory(Category category)
        {
            _context.Categories.Add(category); // Add a new category
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category); // Update the existing category
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            if (category != null)
            {
                _context.Categories.Remove(category); // Remove the category
            }
        }
        public void Save()
        {
            _context.SaveChanges(); // Save changes to the database
        }
    }
}
