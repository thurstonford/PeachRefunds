namespace PeachRefunds.Models
{
    internal class APIPaymentsResponse
    {
        public string? Result { get; set; }
        public string? BatchCode { get; set; }
        public double BatchValueSubmitted { get; set; }
        public double TotalFeeExcludingVAT { get; set; }
        public CDVResults? CDVResults { get; set; }
    }
}
