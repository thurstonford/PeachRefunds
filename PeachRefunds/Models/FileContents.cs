namespace PeachRefunds.Models
{
    internal class FileContents
    {
        public string? Initials { get; set; }
        public string? FirstNames { get; set; }
        public string? Surname { get; set; }
        public string? BranchCode { get; set; }
        public string? AccountNumber { get; set; }
        public double FileAmount { get; set; }
        public string? AccountType { get; set; }
        public byte AmountMultiplier { get; set; }
        public string? CustomerCode { get; set; }
        public string? Reference { get; set; }
    }
}
