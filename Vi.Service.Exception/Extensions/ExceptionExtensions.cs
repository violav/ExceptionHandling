using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Vi.Service.Exception.Extensions
{

    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="DeveloperExceptionPageMiddleware"/>.
    /// </summary>
    ///         /// <summary>
    /// Captures synchronous and asynchronous <see cref="Exception"/> instances from the pipeline and generates HTML error responses.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
    /// <remarks>
    /// This should only be enabled in the Development environment.
    /// </remarks>
    ///

    public static class ExceptionExtensions
    {
        #region "MIDDLEWARE"

        public static IServiceCollection AddExceptions(this IServiceCollection services)
        {
            services
                .AddTransient<ExceptionMapper>();
            return services;
        }

        public static IApplicationBuilder UseDomainDataErrorException(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ExceptionMiddleware>();
        }

        #endregion
    }
}