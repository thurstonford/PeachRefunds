using Newtonsoft.Json;

namespace PeachPayments.Models
{
    public class ResultDetails
    {
        [JsonProperty("ConnectorTxID1")]
        public string? ConnectorTxID1 { get; set; }

        [JsonProperty("ConnectorTxID2")]
        public string? ConnectorTxID2 { get; set; }

        [JsonProperty("AuthorisationCode")]
        public string? AuthorisationCode { get; set; }

        [JsonProperty("AcquirerReference")]
        public string? AcquirerReference { get; set; }

        [JsonProperty("AcquirerResponse")]
        public string? AcquirerResponse { get; set; }
    }
}
