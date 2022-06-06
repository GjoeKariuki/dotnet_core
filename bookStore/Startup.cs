using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using bookStore.Data;
using bookStore.Repository;

namespace bookStore {

    public class Startup {


        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // use this method to add services/dependencies
        public void ConfigureServices(IServiceCollection services) {

            services.AddEntityFrameworkNpgsql().AddDbContext<BookStoreContext>().BuildServiceProvider();

            // services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultString")));
            // identity settings configuration
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BookStoreContext>();

            services.AddControllersWithViews();

            // runtime compilation
#if DEBUG   
            services.AddRazorPages().AddRazorRuntimeCompilation();
            
            // services.AddRazorPages().AddRazorRuntimeCompilation().AddViewOptions(options => {
            //     options.HtmlHelperOptions.ClientValidationEnabled = false;
            // });

            // disabling client-side validation
            // services.AddRazorPages().AddViewOptions(options => {
            // options.HtmlHelperOptions.ClientValidationEnabled = false;
            // });
#endif
            // dependency injection
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();

        

        }

        // configures http pipelines
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

        // endpoints.MapGet("/", async context => {
                //     await context.Response.WriteAsync(env.EnvironmentName);
                // });

            // middleware for static files
            app.UseStaticFiles();

            // app.UseStaticFiles(new StaticFileOptions(){
            //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"MyStaticFolder")),
            //     RequestPath = "/MyStaticFolder"
            // });

            app.UseRouting();

            // authentication
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints => {

                //endpoints.MapControllers();

                endpoints.MapDefaultControllerRoute();
                // endpoints.MapControllerRoute(
                //     name: "Default",
                //     pattern: "bookApp/{controller=Home}/{action=Index}/{id?}"
                // );

            });
        }
    }
}