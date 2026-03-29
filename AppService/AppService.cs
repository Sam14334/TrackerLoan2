using System.Security.Principal;
using Models;
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

        public int CalculateOverdueDays (int daysPassed, int duration)
        {
            return daysPassed - duration;
        }

        public LoanResult ProcessAccount(Account account)
        {
            LoanResult result = new LoanResult();

            if (account == null)
            {
                result.StatusMessage = "Invalid Reference. Please try again.";
                return result;
            }

            result.Account = account;

            int overdueDays = CalculateOverdueDays(account.daysPassed, account.duration);

           

            if(overdueDays> 0)
            {
                result.PenaltyValue = CalculatePenaltyValue(account.amount, account.penaltyRate, overdueDays);
            }
            else
            {
                result.PenaltyValue = 0;
            }

                result.TotalAmount = CalculateTotalAmount(account.amount, result.PenaltyValue);

            if (account.daysPassed >= (account.duration - 5) && account.daysPassed < account.duration)
                result.StatusMessage = "Your loan is almost due";
            else if (account.daysPassed == account.duration)
                result.StatusMessage = "Your loan is due today. Please settle it immediately";
            else if (account.daysPassed > account.duration)
                result.StatusMessage = $"Loan is overdue by {overdueDays} days. Penalty Applied";
            else
                result.StatusMessage = "Your loan is not due yet.";

            return result;
        }

    }
}