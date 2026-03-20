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

            PopulateJsonFile();
        }

        private void PopulateJsonFile()
        {
            RetrieveDataFromJsonFile();

            if (dummyAccounts.Count <= 0)
            {
                dummyAccounts.Add(new Account
                {
                    accountReference = "abcd",
                    daysPassed = 31,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                });

                dummyAccounts.Add(new Account
                {
                    accountReference = "almost",
                    daysPassed = 26,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                });

                dummyAccounts.Add(new Account
                {
                    accountReference = "today",
                    daysPassed = 30,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                });

                dummyAccounts.Add(new Account
                {
                    accountReference = "overdue",
                    daysPassed = 35,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                });

                dummyAccounts.Add(new Account
                {
                    accountReference = "notdue",
                    daysPassed = 10,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                });

                dummyAccounts.Add(new Account
                {
                    accountReference = "edge",
                    daysPassed = 25,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                });

                SaveDataToJsonFile();
            }
        }

        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.OpenWrite(_jsonFileName))
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

        public bool addAccount(Account account)
        {
            if (account != null)
            {
                dummyAccounts.Add(account);
                SaveDataToJsonFile();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Account> getAccounts()
        {
            SaveDataToJsonFile();
            return dummyAccounts;
        }
    }
}
