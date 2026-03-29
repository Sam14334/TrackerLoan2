namespace Models
{
    public class Account 
    {
        public string accountReference { get; set; }
        public int duration { get; set; }
        public int daysPassed { get; set; }
        public int interestRate { get; set; }
        public int penaltyRate { get; set; }
        public double amount { get; set; }
        public double amountToBePaid { get; set; }
    }

}
