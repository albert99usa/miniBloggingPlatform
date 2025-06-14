using Microsoft.EntityFrameworkCore;
using BloggingPlatform.Models;
using Microsoft.AspNetCore.Identity;
using BloggingPlatform.Models.Entity;

namespace BloggingPlatform
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add your DbContext here
            services.AddDbContext<BloggingPlatformContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BlogConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>() // Specify your user type
                .AddEntityFrameworkStores<BloggingPlatformContext>()
                .AddDefaultTokenProviders();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout duration
                //options.Cookie.HttpOnly = true; // Makes the cookie accessible only through HTTP requests
                //options.Cookie.IsEssential = true; // Mark the cookie as essential
            });

            services.AddScoped<IAuthorRepository, SQLAuthorRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            
            services.AddControllersWithViews().WithRazorPagesAtContentRoot().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Author}/{action=Login}/{id?}"); 
            });
        }

    }

}
