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
            else 
            {
                Environment.Exit(0);
            }
        }
        
        private  double CalculatePenaltyValue(double amount, double penaltyRate, int overdueDays)
        {
            if (overdueDays <= 0)
                return 0;

            int penaltyCount = (overdueDays - 1) / 30 + 1;

            return penaltyCount * amount * (penaltyRate / 100.0);
        }

        private double CalculateTotalAmount(double amount, double penaltyValue, int interestRate)
        {
            return amount + ((interestRate*amount)/100) + penaltyValue;
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

            result.TotalAmount = CalculateTotalAmount(account.amount, result.PenaltyValue, account.interestRate);

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

            if(account.amount <= 0 || account.duration <= 0 || account.daysPassed < 0 ||account.penaltyRate <= 0 ||account.interestRate <= 0 || 
                string.IsNullOrEmpty(account.accountReference)|| string.IsNullOrWhiteSpace(account.accountReference))
            {
                return false;
            }
            return dataService.addAccount(account); 

        } 
        public bool ResetAccounts()
        {
           return dataService.resetAccounts();
        }
       
        public bool UpdateAccount(Account account, Account newAccount)
        {
            if (newAccount.amount <= 0 || newAccount.duration <= 0 || newAccount.daysPassed < 0 ||newAccount.penaltyRate <= 0 || newAccount.interestRate <= 0 ||
                string.IsNullOrEmpty(newAccount.accountReference)||string.IsNullOrWhiteSpace(account.accountReference))
            {
                return false;
            }
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