using AppService; 
using DataService;
using Models;
using System.Linq;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TrackerLoan2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Loan Tracking system for an Online banking app, with loan notification and penalties

            //Interface     name            class that implements interface
            //IDataService dataService = new DataJson(); //json data saving
            //IDataService dataService = new DataDB();

            IDataService dataService = new DataService.DataService(); //in-memory data saving
            AppService.AppService appService= new AppService.AppService();
            

            short option1;
            Console.WriteLine("============= Data Saving =============");
            Console.WriteLine("Select a number for your desired data saving method"); 
            Console.WriteLine("[1] In-memory");
            Console.WriteLine("[2] Json file");
            Console.WriteLine("[3] Sql table");
            Console.WriteLine("[4] Exit\n");
            option1 = Convert.ToInt16(Console.ReadLine());

            if (option1 == 2)
            {
                dataService = new DataJson();
            }
            else if (option1 == 3)
            {
                dataService = new DataDB();
            }
            else if (option1 == 4)
            {
                Environment.Exit(0);
            }


            short option2;
            do
            {
                Console.WriteLine("============= Loan Tracker =============");
                Console.WriteLine("Select a number for your desired action");
                Console.WriteLine("[1] Track Loan");
                Console.WriteLine("[2] Add Account (for testing)");
                Console.WriteLine("[3] Exit\n");

                option2 = Convert.ToInt16(Console.ReadLine());

                if (option2 == 1)
                {

                    string referenceInput = getReferenceInput();

                    Account account = dataService.getAccounts().FirstOrDefault(a => a.accountReference == referenceInput);// if input matches any accountReference in dummyAccounts, it will return the first match, otherwise its null.
                   

                    LoanResult result = appService.ProcessAccount(account);

                    if (result.Account == null)
                    {
                        Console.WriteLine(result.StatusMessage);
                    }
                    else
                    {
                        displayLoanInfo( result.Account, result.StatusMessage, result.PenaltyValue, result.TotalAmount);
                    }
                }
                else if (option2 == 2)
                {
                    string accountReference;
                    int daysPassed, duration, interestRate, penaltyRate;
                    double amount = 1000;

                    Console.WriteLine("============= Add Account =============");
                    Console.Write("Reference ID: ");
                    accountReference = Console.ReadLine();
                    Console.Write("Amount (Php): ");
                    amount = double.Parse(Console.ReadLine());
                    Console.Write("DaysPassed since loan: ");
                    daysPassed = int.Parse(Console.ReadLine());
                    Console.Write("Duration of loan: ");
                    duration = int.Parse(Console.ReadLine());
                    Console.Write("Interest Rate (percentage): ");
                    interestRate = int.Parse(Console.ReadLine());
                    Console.Write("Penalty Rate (percentage): ");
                    penaltyRate = int.Parse(Console.ReadLine());


                    Account newAccount = new Account
                    {
                        accountReference = accountReference,
                        amount = amount,
                        daysPassed = daysPassed,
                        duration = duration,
                        interestRate = interestRate,
                        penaltyRate = penaltyRate,
                    };

                    if (dataService.addAccount(newAccount))
                    {
                        Console.WriteLine("Account Added!\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Parameters!\n");
                    }
                 
                }
                else if (option2 == 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid number option\n");
                }
            } while (option2 != 3);
        }
         
        static void displayLoanInfo(Account account,string status, double penaltyValue, double totalAmount)
        {

            Console.WriteLine("============= Loan Status =============");
            Console.WriteLine($"Amount:  {account.amount} Php");
            Console.WriteLine($"Due after:  {account.duration} days");
            Console.WriteLine($"Days Passed Since loan:  {account.daysPassed} days");
            Console.WriteLine($"Interest Rate: {account.interestRate}%");
            Console.WriteLine($"Penalty Rate: {account.penaltyRate}%");
            Console.WriteLine($"Due Date Penalty:  {penaltyValue} Php");
            Console.WriteLine($"Total amount to be paid:  {totalAmount} Php\n");

            Console.WriteLine(status);
        }
        static string getReferenceInput()
        {
            Console.Write("Enter your reference id:");
            string referenceInput = Console.ReadLine();
            return referenceInput;
        }
      


    }
}
