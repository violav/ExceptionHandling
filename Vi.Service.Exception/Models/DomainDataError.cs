using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Vi.Service.Exception.Models
{
    public class DomainDataError : ProblemDetails
    {
        [JsonPropertyName("detail")]
        public IDictionary<string, object?> Details { get; set; }

    }
}