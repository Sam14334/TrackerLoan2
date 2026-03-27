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

                    processAccount(account);
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

        //static double calculatePenaltyValue(double amount, double penaltyRate)
        //{
        //    return amount * (penaltyRate / 100.0);
        //}

        //static double calculateTotalAmount(double amount, double penaltyValue)
        //{
        //    return amount + penaltyValue;
        //}
        static void displayLoanInfo(double amount, int daysPassed, int duration, double interestRate, double penaltyRate, double penaltyValue, double totalAmount)
        {

            Console.WriteLine("============= Loan Status =============");
            Console.WriteLine($"Amount:  {amount} Php");
            Console.WriteLine($"Due after:  {duration} days");
            Console.WriteLine($"Days Passed Since loan:  {daysPassed} days");
            Console.WriteLine($"Interest Rate: {interestRate}%");
            Console.WriteLine($"Penalty Rate: {penaltyRate}%");
            Console.WriteLine($"Due Date Penalty:  {penaltyValue} Php");
            Console.WriteLine($"Total amount to be paid:  {totalAmount} Php\n");
        }
        static string getReferenceInput()
        {
            Console.Write("Enter your reference id:");
            string referenceInput = Console.ReadLine();
            return referenceInput;
        }
        static void processAccount(Account account)
        {
            AppService.AppService appService = new AppService.AppService();

            
            if (account != null)
            {
                int overdueDays = account.daysPassed - account.duration;
                double penaltyValue = 0;
                double totalAmount = 0;

                if (overdueDays > 0)
                {
                    penaltyValue = appService.CalculatePenaltyValue(account.amount, account.penaltyRate, overdueDays); 
                }
                else
                {
                    penaltyValue = 0;
                }
                 
                totalAmount = appService.CalculateTotalAmount(account.amount, penaltyValue);

                if (account.daysPassed >= (account.duration - 5) && account.daysPassed < account.duration)
                {
                    displayLoanInfo(account.amount, account.daysPassed, account.duration, account.interestRate, account.penaltyRate, penaltyValue, totalAmount);
                    Console.WriteLine("Your loan is almost due, please settle it immediately.\n");
                }
                else if (account.daysPassed == account.duration)
                {
                    displayLoanInfo(account.amount, account.daysPassed, account.duration, account.interestRate, account.penaltyRate, penaltyValue, totalAmount);
                    Console.WriteLine("Your loan is due today. Please settle it now.\n");
                }
                else if (account.daysPassed > account.duration)
                {
                    displayLoanInfo(account.amount, account.daysPassed, account.duration, account.interestRate, account.penaltyRate, penaltyValue, totalAmount);
                    Console.WriteLine($"Loan is overdue by {overdueDays} day/s.\nPenalty applied: {penaltyValue} Php");
                }
                else
                {
                    displayLoanInfo(account.amount, account.daysPassed, account.duration, account.interestRate, account.penaltyRate, penaltyValue, totalAmount);
                    Console.WriteLine($"Your loan is not due yet");
                }
                 

            }
            else
            {
                Console.WriteLine("Invalid reference. Please try again.\n");
            }
        }


    }
}
