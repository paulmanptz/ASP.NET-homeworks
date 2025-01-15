using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Cors.Infrastructure;
using PromoCodeFactory.WebHost.Services;

namespace PromoCodeFactory.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddScoped(typeof(IRepository<Employee>), (x) =>
            //    new InMemoryRepository<Employee>(FakeDataFactory.Employees));
            //services.AddScoped(typeof(IRepository<Role>), (x) =>
            //    new InMemoryRepository<Role>(FakeDataFactory.Roles));
            //services.AddScoped(typeof(IRepository<Preference>), (x) =>
            //    new InMemoryRepository<Preference>(FakeDataFactory.Preferences));
            //services.AddScoped(typeof(IRepository<Customer>), (x) =>
            //    new InMemoryRepository<Customer>(FakeDataFactory.Customers));

            var connString = Configuration.GetConnectionString("SqlLiteConnection");
            services.AddDbContext<AppDbContext>(
            options => options.UseSqlite(connString)
            );


            services.AddScoped(typeof(IRepository<Employee>), (x) =>
                new EmployeeEfRepository(x.GetRequiredService<AppDbContext>()));
            services.AddScoped(typeof(IRepository<Role>), (x) =>
                new RoleEfRepository(x.GetRequiredService<AppDbContext>()));
            services.AddScoped(typeof(IExpandedRepository<Customer>), (x) =>
                new CustomerEfRepository(x.GetRequiredService<AppDbContext>()));
            services.AddScoped(typeof(IExpandedRepository<PromoCode>), (x) =>
                new PromoCodeEfRepository(x.GetRequiredService<AppDbContext>()));
            services.AddScoped(typeof(IRepository<Preference>), (x) =>
                new PreferenceEfRepository(x.GetRequiredService<AppDbContext>()));
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IPromoCodeService, PromoCodeService>();


            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
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