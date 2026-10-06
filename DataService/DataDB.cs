//using AppService;
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
    public class DataDB : IDataService
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
                    newDuration = 30,
                    startDate = DateTime.Today.AddDays(-31),
                    dueDate = DateTime.Today.AddDays(-1) 
                };

                Account almostDueAcc = new Account
                {
                    accountReference = "almost",
                    daysPassed = 26,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                    newDuration = 30,
                    startDate = DateTime.Today.AddDays(-26),
                    dueDate = DateTime.Today.AddDays(4) 
                };

                Account dueTodayAcc = new Account
                {
                    accountReference = "today",
                    daysPassed = 30,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                    newDuration = 30,
                    startDate = DateTime.Today.AddDays(-30),
                    dueDate = DateTime.Today 
                };

                Account overdueAcc = new Account
                {
                    accountReference = "overdue",
                    daysPassed = 35,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                    newDuration = 30,
                    startDate = DateTime.Today.AddDays(-35),
                    dueDate = DateTime.Today.AddDays(-5) 
                };

                Account notDueAcc = new Account
                {
                    accountReference = "notdue",
                    daysPassed = 10,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                    newDuration = 30,
                    startDate = DateTime.Today.AddDays(-10),
                    dueDate = DateTime.Today.AddDays(20) 
                };

                Account edgeAcc = new Account
                {
                    accountReference = "edge",
                    daysPassed = 25,
                    duration = 30,
                    amount = 1000,
                    interestRate = 10,
                    penaltyRate = 5,
                    newDuration = 30,
                    startDate = DateTime.Today.AddDays(-25),
                    dueDate = DateTime.Today.AddDays(5) 
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
         
        public List<Account> getAccounts()
        {
            string selectStatement = "SELECT accountReference,duration,daysPassed,interestRate,penaltyRate,amount FROM Accounts";
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


                accounts.Add(account);
            }
            reader.Close();
            sqlConnection.Close();
            return accounts;
        }

        public Account getAccountByReference(string reference)
        {
            return getAccounts().FirstOrDefault(a => a.accountReference.Trim() == reference.Trim());
        }

        public bool addAccount(Account account)
        { 
            if (getAccounts().FirstOrDefault(a => a.accountReference == account.accountReference) != null)
            {
                
                return false;
            } 
            int overdueDays = account.daysPassed - account.duration;

            //New container for datetime overdue days
            int newOverdueDays = (DateTime.Today.Subtract(account.dueDate)).Days;

            Console.WriteLine("newOverdueDays : "+newOverdueDays);

            string insertStatement =
            "INSERT INTO Accounts VALUES(@accountReference, @duration,@daysPassed, @interestRate, @penaltyRate, @amount)";

            //New insertStatement (already in-use)
            string newInsertStatement =
           "INSERT INTO Accounts VALUES(@accountReference, @duration,@daysPassed, @interestRate, @penaltyRate, @amount, @newDuration, @startDate, @dueDate)";

            SqlCommand insertCommand = new SqlCommand(newInsertStatement, sqlConnection);

            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }

            insertCommand.Parameters.AddWithValue("@accountReference", account.accountReference);
            insertCommand.Parameters.AddWithValue("@duration", account.duration);
            insertCommand.Parameters.AddWithValue("@daysPassed", account.daysPassed);
            insertCommand.Parameters.AddWithValue("@interestRate", account.interestRate);
            insertCommand.Parameters.AddWithValue("@penaltyRate", account.penaltyRate);
            //New Insertcommand for new values in inserstatatement
            insertCommand.Parameters.AddWithValue("@newDuration", account.newDuration);
            insertCommand.Parameters.AddWithValue("@startDate", account.startDate);
            insertCommand.Parameters.AddWithValue("@dueDate", account.dueDate);
            insertCommand.Parameters.Add("@amount", SqlDbType.Decimal).Value = account.amount;

            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return true; 
            
        }



        public bool resetAccounts()
        {
            string truncateStatement = "TRUNCATE TABLE Accounts";
            SqlCommand selectCommand = new SqlCommand(truncateStatement, sqlConnection);
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            selectCommand.ExecuteNonQuery();
            populate();
            sqlConnection.Close();
            return true;
        }

        public bool updateAccount(Account account, Account newAccount)
        {
            string updateStatement =
            @" 
            UPDATE Accounts SET accountReference = @accountReference, duration = @duration, daysPassed = @daysPassed, 
            interestRate = @interestRate, penaltyRate = @penaltyRate,  amount = @amount
            WHERE accountReference = @oldAccountReference";

            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            SqlCommand updateCommand = new SqlCommand(updateStatement, sqlConnection);

            updateCommand.Parameters.AddWithValue("@oldAccountReference", account.accountReference);
            updateCommand.Parameters.AddWithValue("@accountReference", newAccount.accountReference);
            updateCommand.Parameters.AddWithValue("@duration", newAccount.duration);
            updateCommand.Parameters.AddWithValue("@daysPassed", newAccount.daysPassed);
            updateCommand.Parameters.AddWithValue("@interestRate", newAccount.interestRate);
            updateCommand.Parameters.AddWithValue("@penaltyRate", newAccount.penaltyRate);
            updateCommand.Parameters.Add("@amount", SqlDbType.Decimal).Value = newAccount.amount;

            updateCommand.ExecuteNonQuery();
            sqlConnection.Close ();
            return true; 
        }

        public bool deleteAccount(Account account)
        {
            string deleteStatement = "DELETE FROM Accounts WHERE accountReference = @accountReference";

            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, sqlConnection);

            deleteCommand.Parameters.AddWithValue("@accountReference", account.accountReference);
            deleteCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return true; 
        }
    }
}
