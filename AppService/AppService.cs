using System.Security.Principal;
using Models;
using DataService;
using Microsoft.VisualBasic;
namespace AppService
{
    public class AppService
    {
        IDataService dataService = new DataDB();
        private readonly EmailService? emailService;

        public AppService()
        {
        }

        public AppService(EmailService emailService)
        {
            this.emailService = emailService;
        }

        public void SendEmailNotification(string accountNumber, string recipientEmail = "borrower@example.com")
        {
            emailService?.SendEmail(accountNumber, recipientEmail);
        }

        //Should've been for choosing which data storing method
        //public AppService (short dataOption)
        //{

        //    if (dataOption == 1)
        //    {
        //        return;
        //    }
        //    else if (dataOption == 2)
        //    {
        //        dataService = new DataJson();
        //    }
        //    else if (dataOption == 3)
        //    {
        //        dataService = new DataDB();  
        //    }
        //    else 
        //    {
        //        Environment.Exit(0);
        //    }
        //}
        
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

        private DateTime InitializeStartDate()
        {
            return DateTime.Today;
        }

        private DateTime CalculateDueDate(DateTime startDate,int newDuration)
        {
            return startDate.AddDays(newDuration);
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

            if (emailService != null)
            {
                try
                {
                    emailService.SendEmail(account.accountReference, "borrower@example.com");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Email Notice]: Could not send email: {ex.Message}");
                }
            }

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
            //newDuration Validation
            if(account.newDuration !=15 && account.newDuration != 30&& account.newDuration != 60)
            {
                return false;
            }

            if (account.amount <= 0 || account.duration <= 0 || account.daysPassed < 0 ||account.penaltyRate <= 0 ||account.interestRate <= 0 || 
                string.IsNullOrEmpty(account.accountReference)|| string.IsNullOrWhiteSpace(account.accountReference))
            {
                return false;
            }

            //create DateTime startDate and DueDate
            account.startDate = InitializeStartDate();
            account.dueDate = CalculateDueDate(account.startDate, account.newDuration);

            //just shows the startdate and duedate
            Console.WriteLine(account.startDate.ToString() + " - " + account.dueDate.ToString());

            bool added = dataService.addAccount(account); 
            if (added && emailService != null)
            {
                try
                {
                    emailService.SendEmail(account.accountReference, "borrower@example.com");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Email Notice]: Could not send email: {ex.Message}");
                }
            }
            return added;
        } 
        public bool ResetAccounts()
        {
           return dataService.resetAccounts();
            //Note: this resets the accounts with its default accounts
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