using System.Security.Principal;
using Models;

namespace DataService
{
    public class DataService
    {
        public List<Account> dummyAccounts = new List<Account>();

        public DataService()
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


            dummyAccounts.Add(dummyAcc1);
            dummyAccounts.Add(almostDueAcc);
            dummyAccounts.Add(dueTodayAcc);
            dummyAccounts.Add(overdueAcc);
            dummyAccounts.Add(notDueAcc);
            dummyAccounts.Add(edgeAcc);
        }
        public bool addAccount(Account account)
        {

            dummyAccounts.Add(account);
            return true;
        }

        public List<Account> getAccounts()
        {
            return dummyAccounts;
        }
    }
}
