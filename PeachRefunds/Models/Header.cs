namespace PeachRefunds.Models
{
    internal class Header
    {
        public string? PsVer { get;set; }
        public string? Client { get; set; }
        public string? DueDate { get; set; }
        public string? Service { get; set; }
        public string? ServiceType { get; set; }
        public string? Reference { get; set; }
        public string? CallBackUrl { get; set; }        
    }
}
