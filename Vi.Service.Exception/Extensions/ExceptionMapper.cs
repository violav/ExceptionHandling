using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using Vi.Service.Exception.Models;

namespace Vi.Service.Exception.Extensions
{
    public class ExceptionMapper
    {
        public DomainDataError Map(HttpContext httpContext, System.Exception exc)
        {
            var status = httpContext.Response.StatusCode;
            var title = ReasonPhrases.GetReasonPhrase(status);
            var instance = httpContext.Request.Path;
            var type = string.Concat("https://httpstatuses.io/", status);
            var errors = new Dictionary<string, object?>();
            var data = exc.Data.GetEnumerator();
            while (data.MoveNext())
            {
                errors.Add(data.Key.ToString(), data.Value);
            }

            var excStatus = HttpStatusCode.InternalServerError;
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            var details = exc.Message;

            var problemDetails = new DomainDataError()
            {
                Details = errors,
                Detail = details,
                Extensions = { },
                Instance = instance,
                Status = (int)excStatus,
                Title = title,
                Type = type
            };
            return problemDetails;
        }

        public string MapToJson(HttpContext httpContext, System.Exception exc)
        {
            var domainDataError = Map(httpContext, exc);
            return JsonConvert.SerializeObject(domainDataError);
        }
    }
}
