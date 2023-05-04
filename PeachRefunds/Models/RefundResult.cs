using Newtonsoft.Json;

namespace PeachPayments.Models
{
    public class RefundResult
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("referencedId")]
        public string? ReferencedId { get; set; }

        [JsonProperty("paymentType")]
        public string? PaymentType { get; set; }

        [JsonProperty("amount")]
        public string? Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("descriptor")]
        public string? Descriptor { get; set; }

        [JsonProperty("merchantTransactionId")]
        public string? MerchantTransactionId { get; set; }

        [JsonProperty("result")]
        public Result? Result { get; set; }

        [JsonProperty("resultDetails")]
        public ResultDetails? ResultDetails { get; set; }

        [JsonProperty("buildNumber")]
        public string? BuildNumber { get; set; }

        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("ndc")]
        public string? NDC { get; set; }

        [JsonIgnore]
        public bool IsSuccessful {
            get {
                switch(this?.Result?.Code) {
                    case "000.000.000": //Transaction successfully processed in LIVE system
                    case "000.100.110": //Transaction successfully processed in TEST system                                    
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
