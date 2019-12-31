using Core.Database;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
#if DEBUG
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
#else
                    //builder.WithOrigins("http://traveller-frontend.azurewebsites.net").AllowAnyHeader().AllowAnyMethod();
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
#endif

                });
            });
            services.AddDbContext<AppDbContext>(options => options.UseMySql("server=traveller-db.cxg6ocbiidbi.ap-southeast-2.rds.amazonaws.com;database=Traveller;user=admin;password=Qwerty1234", b => b.MigrationsAssembly("API")));
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=traveller-database.cxg6ocbiidbi.ap-southeast-2.rds.amazonaws.com;Database=Traveller;User Id=admin;Password=Qwerty1234!;", b => b.MigrationsAssembly("API")));

            services.AddScoped<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
