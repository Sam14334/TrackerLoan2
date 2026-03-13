using DataService;
using Models;
using AppService;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TrackerLoan2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Loan Tracking system for an Online banking app, with loan notification and penalties
               

            DataService.DataService dataService = new DataService.DataService();
            AppService.AppService appService = new AppService.AppService();

            short option;
            do {
                Console.WriteLine("============= Loan Tracker =============");
                Console.WriteLine("Select a number for your desired action");
                Console.WriteLine("[1] Track Loan");
                Console.WriteLine("[2] Add Account (for testing)");
                Console.WriteLine("[3] Exit\n");

                option = Convert.ToInt16( Console.ReadLine());

                if (option == 1) {

                    string referenceInput = getReferenceInput(); 

                    Account account = dataService.dummyAccounts.FirstOrDefault(a => a.accountReference == referenceInput);// if input matches any accountReference in dummyAccounts, it will return the first match, otherwise its null.

                    processAccount(account);
                } else if (option == 2)
                {
                    dataService.addAccount(new Account { accountReference = "123", amount = 2500, daysPassed = 21, duration = 100, interestRate = 12, penaltyRate = 6 });
                } else if (option == 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid number option\n");
                }
            } while (option!= 3);
        }

        static double calculatePenaltyValue(double amount, double penaltyRate)
        {
            return amount * (penaltyRate/100.0);
        }
        static void displayLoanInfo(double amount,int daysPassed, int duration, double interestRate, double penaltyValue, double penaltyRate)
        {
            
            Console.WriteLine("============= Loan Status ============="); 
            Console.WriteLine($"Amount:  {amount} Php"); 
            Console.WriteLine($"Due after:  {duration} days"); 
            Console.WriteLine($"Days Passed Since loan:  {daysPassed} days"); 
            Console.WriteLine($"Interest Rate: {interestRate}%");
            Console.WriteLine($"Penalty Rate: {penaltyRate}%");
            Console.WriteLine($"Due Date Penalty:  {penaltyValue} Php\n");
        }
        static string getReferenceInput()
        {
            Console.Write("Enter a reference id:");
            string referenceInput = Console.ReadLine();
            return referenceInput;
        }
        static void processAccount(Account account)
        {
            double penaltyValue = 0;
            if (account != null)
            {
                penaltyValue = calculatePenaltyValue(account.amount, account.penaltyRate);
                displayLoanInfo(account.amount, account.daysPassed, account.duration, account.interestRate, penaltyValue, account.penaltyRate);

                if (account.daysPassed > account.duration)
                {
                    Console.WriteLine($"Loan is overdue by {account.daysPassed - account.duration} day/s.\nPenalty applied: {penaltyValue} Php");
                }
                else if (account.daysPassed >= (account.duration - 5))
                {
                    Console.WriteLine("Your loan is almost due, please settle it immediately.\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid reference. Please try again.\n");
            }
        }


    }
}
    