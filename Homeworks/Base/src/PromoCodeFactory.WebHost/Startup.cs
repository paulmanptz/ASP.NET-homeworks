using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PromoCodeFactory.WebHost
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
            services.AddControllers();
            //services.AddSingleton(typeof(IRepository<Employee>), (x) => 
            //    new InMemoryRepositoryList<Employee>(FakeDataFactory.Employees.ToList()));
            //services.AddSingleton(typeof(IRepository<Role>), (x) => 
            //    new InMemoryRepositoryList<Role>(FakeDataFactory.Roles.ToList()));

            var connString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlite(connString)
                );


            //services.AddOpenApiDocument(options =>
            //{
            //    options.Title = "PromoCode Factory API Doc";
            //    options.Version = "1.0";
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}