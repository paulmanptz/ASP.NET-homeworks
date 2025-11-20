using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.DataAccess;
using Pcf.Administration.DataAccess.Data;
using Pcf.Administration.DataAccess.Repositories;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Pcf.Administration.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services.AddControllers().AddMvcOptions(x =>
                x.SuppressAsyncSuffixInActionNames = false);

            services.Configure<MongoDBSettings>(Configuration.GetSection(nameof(MongoDBSettings)));

            services.AddSingleton<IMongoClient>(sp =>
            {
                MongoDBSettings settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

                return new MongoClient(settings.Connection);
            });

            services.AddScoped(sp =>
            {
                IMongoClient client = sp.GetRequiredService<IMongoClient>();
                MongoDBSettings settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });

            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IDbInitializer, MongoDbInitializer>();

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory Administration API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
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

            dbInitializer.InitializeDb();
        }
    }
}