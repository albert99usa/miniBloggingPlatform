using BloggingPlatform.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatform.Models
{
    public class BloggingPlatformContext : IdentityDbContext<ApplicationUser>
    {
        public BloggingPlatformContext(DbContextOptions<BloggingPlatformContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AuthorBlogLike> AuthorBlogLike { get; set; }
        public DbSet<Report> Reports { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AuthorBlogLike>()
            .HasKey(ab => new { ab.AuthorId, ab.BlogId }); // Composite key

            modelBuilder.Entity<AuthorBlogLike>()
                .HasOne(ab => ab.Author)
                .WithMany(a => a.LikedBlogs)
                .HasForeignKey(ab => ab.AuthorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuthorBlogLike>()
                .HasOne(ab => ab.Blog)
                .WithMany(b => b.LikedByAuthors)
                .HasForeignKey(ab => ab.BlogId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Blog)
                .WithMany(b => b.Reports) 
                .HasForeignKey(r => r.BlogId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Author)
                .WithMany(a => a.Reports) 
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Author>().ToTable("Author");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Blog>().ToTable("Blog");
            modelBuilder.Entity<AuthorBlogLike>().ToTable("AuthorBlogLike");

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology", Slug = "technology" },
                new Category { Id = 2, Name = "Health", Slug = "health" },
                new Category { Id = 3, Name = "Travel", Slug = "travel" },
                new Category { Id = 4, Name = "Food", Slug = "food" },
                new Category { Id = 5, Name = "Fashion", Slug = "fashion" },
                new Category { Id = 6, Name = "Lifestyle", Slug = "lifestyle" },
                new Category { Id = 7, Name = "Business", Slug = "business" },
                new Category { Id = 8, Name = "Education", Slug = "education" },
                new Category { Id = 9, Name = "Entertainment", Slug = "entertainment" },
                new Category { Id = 10, Name = "Sports", Slug = "sports" }
            );

        }
    }
}
