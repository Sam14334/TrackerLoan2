using System.Security.Principal;
using Models;
using DataService;
namespace AppService
{
    public class AppService
    {
        IDataService dataService = new DataService.DataService();
        public AppService()
        {

        }
        public AppService (short dataOption)
        {
            
            if (dataOption == 2)
            {
                dataService = new DataJson();
            }
            else if (dataOption == 3)
            {
                dataService = new DataDB();
            }
            else if (dataOption == 4)
            {
                Environment.Exit(0);
            }
        }
        
        private  double CalculatePenaltyValue(double amount, double penaltyRate, int overdueDays)
        {
            if (overdueDays <= 0) return 0;
            return amount * (penaltyRate / 100.0) * overdueDays;
        }

        private double CalculateTotalAmount(double amount, double penaltyValue)
        {
            return amount + penaltyValue;
        }

        private int CalculateOverdueDays (int daysPassed, int duration)
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

        public Account GetAccountByReference(string refInput)
        {
           return dataService.getAccountByReference(refInput);
        }

        public List<Account> GetAllAccounts()
        {
            
            return dataService.getAccounts();

        }
        public bool RegisterAccount(Account account)
        { 


            return dataService.addAccount(account); 

        } 
        public bool ResetAccounts()
        {
           return dataService.resetAccounts();
        }
       
        public bool UpdateAccount(Account account, Account newAccount)
        {;
            return dataService.updateAccount(account, newAccount);
             
        }

        public bool DeleteAccount(Account account)
        {
            if(account == null)
            {
                return false;
            }
            return dataService.deleteAccount(account);
        }
         
    }
}