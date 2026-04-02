using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataService
{

    public class DataJson : IDataService
    {
        private List<Account> dummyAccounts = new List<Account>();

        private string _jsonFileName;
        public DataJson()
        { 
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/Accounts.json";

            populate();
            
        }

        public void populate()
        {
            RetrieveDataFromJsonFile();

            if (dummyAccounts.Count <= 0)
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

                SaveDataToJsonFile();
            }
        }

        public List<Account> getAccounts()
        {
            RetrieveDataFromJsonFile();
            return dummyAccounts;
        }

        public Account getAccountByReference(string reference)
        {
            return getAccounts().FirstOrDefault(a => a.accountReference == reference);
        }

        public bool addAccount(Account account)
        { 

            if (getAccounts().FirstOrDefault(a => a.accountReference == account.accountReference)!= null)
            {
                
                return false;
            }
            //int overdueDays = account.daysPassed - account.duration;

            dummyAccounts.Add(account);
            SaveDataToJsonFile();
            return true;
            
        }


        public bool resetAccounts()
        {
            dummyAccounts.Clear();
            SaveDataToJsonFile();

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
            SaveDataToJsonFile();
            return true;
        }

        public bool deleteAccount(Account account)
        {
            dummyAccounts.Remove(account);
            SaveDataToJsonFile();
            return true;
        }


        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.Create(_jsonFileName))
            {
                JsonSerializer.Serialize<List<Account>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    { SkipValidation = true, Indented = true })
                    , dummyAccounts);
            }
        }

        private void RetrieveDataFromJsonFile()
        {

            using (var jsonFileReader = File.OpenText(this._jsonFileName))
            {
                this.dummyAccounts = JsonSerializer.Deserialize<List<Account>>
                    (jsonFileReader.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true })
                    .ToList();
            }
        }
    }
}
