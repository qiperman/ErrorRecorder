using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // get the connection string from the configuration file
            string connection = Configuration.GetConnectionString("DefaultConnection");
            //add the context as a service to the application
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connection));

            // setup connection configuration
            services.AddAuthentication("Auth")
                .AddCookie("Auth",options => //CookieAuthenticationOptions
                {
                    options.LoginPath = "/Account/Login";
                });

            services.AddTransient<IErrorRepository, EFErrorRepository>();
            services.AddTransient<IUserRepository, EFUsersRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseBrowserLink();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Error}/{action=List}/{id?}");
            });

 
            var dbContext = serviceProvider.GetService<ApplicationDbContext>();

            SeedData.EnsurePopulated(dbContext);
        }
    }
}
