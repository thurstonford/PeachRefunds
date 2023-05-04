using Newtonsoft.Json;

namespace PeachPayments.Models
{
    public class Result
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}
