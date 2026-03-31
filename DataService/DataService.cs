using System.Security.Principal;
using Models;

namespace DataService
{
    public class DataService : IDataService
    {
        public List<Account> dummyAccounts = new List<Account>();

        public DataService()
        {
            populate();
        }
        public void populate()
        {

            Account dummyAcc1 = new Account
            {
                accountReference = "abcd",
                daysPassed = 31,
                duration = 30,
                amount = 1000,
                interestRate = 10,
                penaltyRate = 5,
            };

            Account almostDueAcc = new Account
            {
                accountReference = "almost",
                daysPassed = 26,
                duration = 30,
                amount = 1000,
                interestRate = 10,
                penaltyRate = 5,
            };

            Account dueTodayAcc = new Account
            {
                accountReference = "today",
                daysPassed = 30,
                duration = 30,
                amount = 1000,
                interestRate = 10,
                penaltyRate = 5,
            };

            Account overdueAcc = new Account
            {
                accountReference = "overdue",
                daysPassed = 35,
                duration = 30,
                amount = 1000,
                interestRate = 10,
                penaltyRate = 5,
            };

            Account notDueAcc = new Account
            {
                accountReference = "notdue",
                daysPassed = 10,
                duration = 30,
                amount = 1000,
                interestRate = 10,
                penaltyRate = 5,
            };

            Account edgeAcc = new Account
            {
                accountReference = "edge",
                daysPassed = 25,
                duration = 30,
                amount = 1000,
                interestRate = 10,
                penaltyRate = 5,
            };


            addAccount(dummyAcc1);
            addAccount(almostDueAcc);
            addAccount(dueTodayAcc);
            addAccount(overdueAcc);
            addAccount(notDueAcc);
            addAccount(edgeAcc);
        }
        public bool addAccount(Account account)
        {
             
            if (getAccounts().FirstOrDefault(a => a.accountReference == account.accountReference)!=null)
            {
                return false;
            }
            dummyAccounts.Add(account);
            return true;
        }

        public List<Account> getAccounts()
        {
            return dummyAccounts; 
        }
         
        public bool resetAccounts()
        { 
            dummyAccounts.Clear();
            populate();
            return true;
             
        }

        public bool updateAccount(Account account, Account newAccount)
        {
             
            account.accountReference = newAccount.accountReference;
            account.amount = newAccount.amount;
            account.daysPassed = newAccount.daysPassed;
            account.duration = newAccount.duration;
            account.interestRate = newAccount.interestRate;
            account.penaltyRate = newAccount.penaltyRate;
            return true;
              
        }

        public bool deleteAccount(Account account)
        {
            dummyAccounts.Remove(account);
            return true;
        }
    }
}
