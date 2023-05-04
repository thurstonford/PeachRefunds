namespace PeachPayments.Models
{
    public class Refund
    {
        /// <summary>
        /// Internal identifier for client
        /// </summary>
        public string? Id { get; set; }
        public double Amount { get;set; }
        public string? TransactionId { get; set; }
        public string? Currency { get; set; }
    }
}
