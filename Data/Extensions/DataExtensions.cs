using Data.Data.EF.Context;
using Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Data.Extensions
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataContext2(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions()
                .Configure<DataOptions>(configuration.GetSection("DataOptions"));

            services
                .AddDbContext<NorthwindContext>((serviceProvider, opt) =>
                {
                    var securityOptions = serviceProvider.GetService<IOptions<DataOptions>>()?.Value;
                    opt.UseSqlServer(securityOptions.Connectionstring);
                });

            return services;
        }
    }
}
