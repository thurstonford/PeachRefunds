namespace PeachPayments.Models
{
    public class RefundConfig
    {
        public byte Id { get; set; }    
        public string? EntityId { get; set; }
        public string? Secret { get; set; }
        public bool IsActive { get; set; }
    }
}