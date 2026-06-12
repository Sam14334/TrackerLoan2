namespace Models
{
    public class AccountViewModel
    {
        public string? accountReference { get; set; }
        public int duration { get; set; }
        public int daysPassed { get; set; }
        public int interestRate { get; set; }
        public int penaltyRate { get; set; }
        public double amount { get; set; }
    }
}