using BloggingPlatform.Models.Entity;

namespace BloggingPlatform.Models
{
    public interface IAuthorRepository
    {
        Author GetAuthorById(int id);
        IEnumerable<Author> GetAllAuthors();
        Author Add(Author author);
        Author Update(Author authorChages);
        Author Delete(int id);
        Author GetAuthorByEmailAndPassword(string email, string password);
        Author GetAuthorByEmail(string email);
        IEnumerable<Blog> GetLikedBlogsByAuthorId(int authorId);
        void save();

    }
}
