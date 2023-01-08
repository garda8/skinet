using Infastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddScoped<IProductsRepository, ProductsRepository>();
            //services.AddTransient<IProductsRepository, ProductsRepository>();  //för kort levnadstid , för bara en metod.
            //services.AddSingleton<IProductsRepository, ProductsRepository>();  //för lång levnadstid, typ så länge appen är igång..

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

           


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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
