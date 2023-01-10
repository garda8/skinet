using API.Errors;
using Infastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductsRepository, ProductsRepository>();
            //services.AddTransient<IProductsRepository, ProductsRepository>();  //för kort levnadstid , för bara en metod.
            //services.AddSingleton<IProductsRepository, ProductsRepository>();  //för lång levnadstid, typ så länge appen är igång..


            services.Configure<ApiBehaviorOptions>(options =>

            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;

        }
    }
}
