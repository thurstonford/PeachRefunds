using Newtonsoft.Json;

namespace PeachPayments.Models
{
    public class ErrorResponse
    {
        [JsonProperty("result.code")]
        public string? Code { get; set; }

        [JsonProperty("result.description")]
        public string? Description { get; set; }

        [JsonProperty("result.parameterErrors")]
        public List<ParameterError>? ParameterErrors { get; set; }
    }
}
