namespace PeachRefunds.Models
{
    internal class APIPaymentsRequest
    {
        public Header? Header { get; set; }
        public Payments? Payments { get; set; }
        public Totals? Totals { get; set; }
    }
}
