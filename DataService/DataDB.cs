using AppService;
using Microsoft.Data.SqlClient;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    internal class DataDB : IDataService
    {
        private string connectionString = "Data Source=localhost\\SQLEXPRESS02;Initial Catalog=LoanTracker;Integrated Security=True;TrustServerCertificate=True;";

        private SqlConnection sqlConnection;

        public DataDB()
        {
            sqlConnection = new SqlConnection(connectionString);    
            populate();
            sqlConnection.Close();
        }

        public void populate()
        {
            Console.WriteLine("SQL Data Method");

            List<Account> currentAccounts = getAccounts();

            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }

            if (currentAccounts.Count <= 0)
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

            sqlConnection.Close();
            
        }

        //private void addPopulate(string accountReference, int duration, int daysPassed, int interestRate, int penaltyRate, double amount, double amountToBePaid)
        //{
        //    string insertStatement =
        //    "INSERT INTO Accounts VALUES(@accountReference, @duration,@daysPassed, @interestRate, @penaltyRate, @amount, @amountToBePaid)";

        //    SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

        //    insertCommand.Parameters.AddWithValue("@accountReference", accountReference);
        //    insertCommand.Parameters.AddWithValue("@duration", duration);
        //    insertCommand.Parameters.AddWithValue("@daysPassed", daysPassed);
        //    insertCommand.Parameters.AddWithValue("@interestRate", interestRate);
        //    insertCommand.Parameters.AddWithValue("@penaltyRate", penaltyRate);
        //    insertCommand.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;
        //    insertCommand.Parameters.Add("@amountToBePaid", SqlDbType.Decimal).Value = amountToBePaid;

        //    insertCommand.ExecuteNonQuery();

           
        //}
        public bool addAccount(Account account)
        {
            AppService.AppService appService = new AppService.AppService();

            if (account != null)
            {
                int overdueDays = account.daysPassed - account.duration;
                double penaltyValue = appService.CalculatePenaltyValue(account.amount, account.penaltyRate, overdueDays);
                account.amountToBePaid = appService.CalculateTotalAmount(account.amount, penaltyValue);

                string insertStatement =
                "INSERT INTO Accounts VALUES(@accountReference, @duration,@daysPassed, @interestRate, @penaltyRate, @amount, @amountToBePaid)";

                SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                insertCommand.Parameters.AddWithValue("@accountReference", account.accountReference);
                insertCommand.Parameters.AddWithValue("@duration", account.duration);
                insertCommand.Parameters.AddWithValue("@daysPassed", account.daysPassed);
                insertCommand.Parameters.AddWithValue("@interestRate", account.interestRate);
                insertCommand.Parameters.AddWithValue("@penaltyRate", account.penaltyRate);
                insertCommand.Parameters.Add("@amount", SqlDbType.Decimal).Value = account.amount;
                insertCommand.Parameters.Add("@amountToBePaid", SqlDbType.Decimal).Value = account.amountToBePaid;

                insertCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
            else { 
                return false; 
            }
        }


        public List<Account> getAccounts()
        {
            string selectStatement = "SELECT accountReference,duration,daysPassed,interestRate,penaltyRate,amount,amountToBePaid FROM Accounts";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }

            SqlDataReader reader = selectCommand.ExecuteReader();

            List<Account> accounts = new List<Account>();

            while (reader.Read())
            {

                Account account = new Account();
                account.accountReference = reader["accountReference"].ToString();
                account.duration = Convert.ToInt32(reader["duration"]);
                account.daysPassed = Convert.ToInt32(reader["daysPassed"]);
                account.interestRate = Convert.ToInt32(reader["interestRate"]);
                account.penaltyRate = Convert.ToInt32(reader["penaltyRate"]);
                account.amount = Convert.ToDouble(reader["amount"]);
                account.amountToBePaid = Convert.ToDouble(reader["amountToBePaid"]);


                accounts.Add(account);
            }
            reader.Close();
            sqlConnection.Close();
            return accounts;
        }
    }
}
