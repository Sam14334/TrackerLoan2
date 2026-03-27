namespace AppService
{
    public class AppService
    {
        public  double CalculatePenaltyValue(double amount, double penaltyRate, int overdueDays)
        {
            if (overdueDays <= 0) return 0;
            return amount * (penaltyRate / 100.0) * overdueDays;
        }

        public  double CalculateTotalAmount(double amount, double penaltyValue)
        {
            return amount + penaltyValue;
        }
    }
}