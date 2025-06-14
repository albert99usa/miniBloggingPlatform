using BloggingPlatform.Models.Entity;

namespace BloggingPlatform.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories(); // Get all categories
        Category GetCategoryById(int id); // Get a category by ID
        void CreateCategory(Category category); // Create a new category
        void UpdateCategory(Category category); // Update an existing category
        void DeleteCategory(int id); // Delete a category by ID
        void Save(); // Save changes to the database
    }
}
