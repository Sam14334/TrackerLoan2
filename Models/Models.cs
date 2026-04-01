namespace Models
{
    public class Account 
    {
        public string  accountReference { get; set; }
        public int duration { get; set; }
        public int daysPassed { get; set; }
        public int interestRate { get; set; }
        public int penaltyRate { get; set; }
        public double amount { get; set; } 
    }

    public class LoanResult
    {
        public Account Account { get; set; }
        public string StatusMessage { get; set; }
        public double PenaltyValue { get; set; }
        public double TotalAmount { get; set; }
    }

}
