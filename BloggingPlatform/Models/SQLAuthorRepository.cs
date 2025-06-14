using BloggingPlatform.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace BloggingPlatform.Models
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly BloggingPlatformContext context;
        public SQLAuthorRepository(BloggingPlatformContext context)
        {
            this.context = context;
        }
        Author IAuthorRepository.Add(Author author)
        {
            context.Authors.Add(author);
            
            return author;
        }
        Author IAuthorRepository.Delete(int id)
        {
            Author author = context.Authors.Find(id);
            if(author != null)
            {
                context.Authors.Remove(author);
                
            }
            return author;
        }

        IEnumerable<Author> IAuthorRepository.GetAllAuthors()
        {
            return context.Authors;
        }

        Author IAuthorRepository.GetAuthorById(int id)
        {
            return context.Authors.FirstOrDefault(m => m.ID == id);
        }

        Author IAuthorRepository.Update(Author authorChages)
        {
            var author = context.Authors.Attach(authorChages);
            author.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
           
            return authorChages;
        }

        Author IAuthorRepository.GetAuthorByEmailAndPassword(string email, string password)
        {
            return context.Authors.FirstOrDefault(a => a.Email == email && a.Password == password);
        }

        Author IAuthorRepository.GetAuthorByEmail(string email)
        {
            return context.Authors.FirstOrDefault(a => a.Email == email);
        }

        IEnumerable<Blog> IAuthorRepository.GetLikedBlogsByAuthorId(int authorId)
        {
            return context.AuthorBlogLike
                           .Where(l => l.AuthorId == authorId)
                           .Select(l => l.Blog)
                           .ToList();
        }
        void IAuthorRepository.save()
        {
            context.SaveChanges();
        }
    }
}
