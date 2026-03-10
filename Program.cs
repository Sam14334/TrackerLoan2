using DataService;
using Models;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TrackerLoan2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Loan Tracking system for an Online banking app, with loan notification and penalty.
             
           
            double penaltyValue=0;
            double penaltyRate=0;

            DataService.DataService dataService = new DataService.DataService();

            string referenceInput = getReferenceInput();

            Account account = dataService.dummyAccounts.FirstOrDefault(a => a.accountReference == referenceInput);// if input matches any accountReference in dummyAccounts, it will return the first match, otherwise its null.

            processAccount(account,penaltyValue);

           

            
        }

        static double calculatePenaltyValue(double amount, double penaltyRate)
        {
            return amount * (penaltyRate/100.0);
        }
        
        static string getReferenceInput()
        {
            Console.Write("Enter a reference id:");
            string referenceInput = Console.ReadLine();
            return referenceInput;
        }

        static void displayLoanInfo(int noOfLoans, double amount,int daysPassed, int duration, double interestRate, double penaltyValue, double penaltyRate)
        {
            
            Console.WriteLine($"No of loans:  {noOfLoans}");
            Console.WriteLine($"Amount:  {amount} Php"); 
            Console.WriteLine($"Due after:  {duration} days"); 
            Console.WriteLine($"Days Passed Since loan:  {daysPassed} days"); 
            Console.WriteLine($"Interest Rate: {interestRate}%");
            Console.WriteLine($"Penalty Rate: {penaltyRate}%");
            Console.WriteLine($"Due Date Penalty:  {penaltyValue} Php");
        }

       static void processAccount(Account account, double penaltyValue)
        {
            if (account != null)
            {
                penaltyValue = calculatePenaltyValue(account.amount, account.penaltyRate);
                displayLoanInfo(account.noOfLoans, account.amount, account.daysPassed, account.duration, account.interestRate, penaltyValue, account.penaltyRate);

                if (account.daysPassed > account.duration)
                {
                    Console.WriteLine($"Loan is overdue by {account.daysPassed - account.duration} \nday/s. Penalty applied: {penaltyValue} Php");
                }
                else if (account.daysPassed >= (account.duration - 5))
                {
                    Console.WriteLine("Your loan is almost due, please settle it immediately.");
                }
            }
            else
            {
                Console.WriteLine("Invalid reference. Please try again.");
            }
        }
    }
}
    