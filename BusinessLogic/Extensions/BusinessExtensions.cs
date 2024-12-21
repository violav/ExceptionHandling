using BusinessLogic.Controller;
using BusinessLogic.Options;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessLogic.Extensions
{
    public static class BusinessExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions()
                .Configure<BusinessOptions>(configuration.GetSection("BusinessOptions"));

            services
                .AddScoped<BusinessController>()
                .AddScoped<BusinessServices>();

            return services;
        }
    }
}
