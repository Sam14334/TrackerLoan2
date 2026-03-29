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

        public (Account,string, double,double)  ProcessAccount(Account account)
        {
            string status = "Invalid Reference Please try again";
            double penaltyValue =0;
            double totalAmount =0;
            if (account != null)
            {
                int overdueDays = account.daysPassed - account.duration; 
                

                if (overdueDays > 0)
                {
                    penaltyValue = CalculatePenaltyValue(account.amount, account.penaltyRate, overdueDays);
                }
                else
                {
                    penaltyValue = 0;
                }

                totalAmount = CalculateTotalAmount(account.amount, penaltyValue);

                if (account.daysPassed >= (account.duration - 5) && account.daysPassed < account.duration)
                {
                    status = "Your loan is almost due, please settle it immediately.\n";
                    return (account, status, penaltyValue,totalAmount); 
                }
                else if (account.daysPassed == account.duration)
                {
                    status = "Your loan is due today. Please settle it now.\n";
                    return (account, status,penaltyValue, totalAmount); 
                }
                else if (account.daysPassed > account.duration)
                {
                    status = $"Loan is overdue by {overdueDays} day/s.\nPenalty applied: {penaltyValue} Php.\n";
                    return (account, status,penaltyValue, totalAmount); 
                }
                else
                {
                    status = "Your loan is not due yet.\n";
                    return (account, status, penaltyValue, totalAmount);
                }


            }
            else
            {
                return (account,status,penaltyValue, totalAmount);
            }
        }
    }
}