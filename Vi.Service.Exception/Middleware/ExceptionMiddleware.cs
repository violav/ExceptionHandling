using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Vi.Service.Exception.Extensions;

namespace Vi.Service.Exception
{
    /// <summary>
    /// Captures synchronous and asynchronous exceptions from the pipeline and generates error responses.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly ExceptionMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            ExceptionMapper mapper
            )
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogCritical(ex, $"Response has been already started ({ex.Message})");
                    throw;
                }
                try
                {
                    _logger.LogCritical(ex, $"{ex.Source}: {ex.Message}");
                    var jsonResponse = _mapper.MapToJson(context, ex);

                    context.Response.Clear();
                    context.Response.StatusCode = 500; 
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResponse);
                }
                catch (System.Exception innerException)
                {
                    _logger.LogCritical(ex, $"Unhandled exception in {(nameof(ExceptionMiddleware))} ({innerException.Message})");
                }
            }
        }
    }
}
