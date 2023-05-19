namespace PeachRefunds.Models
{
    internal class Totals
    {
        public int Records { get; set; }

        public double Amount { get; set; }

        public string? BranchHash { get; set; }

        public string? AccountHash { get; set; }
    }
}
