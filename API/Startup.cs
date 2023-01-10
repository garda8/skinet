using API.Extensions;
using API.Helpers;
using API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllers();

            services.AddApplicationServices();  
            //Hit har jag flyttat AddScoped<IProductRepository, ProductRepository>
            //och även services.Configure..

            services.AddSwaggerDocumentation();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            
            if (env.IsDevelopment())
            {
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        ////private void SetupDependencyInjection(IServiceCollection services)
        ////{
        ////    services.AddTransient<IDbConnection>((sp) =>
        ////    {
        ////        return new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
        ////    });

        ////    //services.AddTransient<IProductsRepository, ProductsRepository>();
            
        ////    ////services.AddTransient<IFarmService, FarmService>();
        ////    ////services.AddTransient<IFarmRepository, FarmRepository>();
        ////    ////services.AddTransient<IAnimalService, AnimalService>();
        ////    ////services.AddTransient<IAnimalRepository, AnimalRepository>();
        ////    ////services.AddTransient<IFileService, FileService>();
        ////    ////services.AddTransient<IFileRepository, FileRepository>();

        ////    //services.AddTransient<IActiveDirectoryProvider, ActiveDirectoryProvider>();


        ////    SqlMapperExtensions.TableNameMapper = (type) => { return type.Name; };
        ////}
    }
}
