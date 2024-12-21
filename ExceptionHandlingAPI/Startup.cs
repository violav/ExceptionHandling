using BusinessLogic.Extensions;
using Data.Extensions;
using Vi.Service.Exception.Extensions;

namespace violaAPITest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions();

            services
                .AddCors()
                .AddControllers();

            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();                
                //.AddClass();
            services.AddDataContext2(Configuration);

            services
                .AddBusiness(Configuration)
                .AddExceptions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDomainDataErrorException();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
