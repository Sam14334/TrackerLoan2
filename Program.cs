namespace TrackerLoan2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string exampleReference = "abcd";
            int dayPassed = 31;
            int dueDate = 30;

            int noOfLoans = 1;
            int amount = 1000;
            int duration = 60;
            double interestRate = 10;
            double penaltyValue = amount + (amount * interestRate);


            string referenceInput = getReferenceInput();
             
            if (referenceInput == exampleReference)
            {
                displayLoanInfo(noOfLoans, amount, duration, interestRate, penaltyValue); 
            }
            else
            {
                Console.WriteLine("Invalid reference. Please try again.");
            }

            if (dayPassed > dueDate)
            {
                Console.WriteLine($"Loan is overdue by {dayPassed - dueDate} day/s. Penalty applied: {penaltyValue}");
            }
            else if (dayPassed == (dueDate - 5))
            {
                Console.WriteLine("Your loan is almost due, please settle it immediately.");
            }
        }

        static string getReferenceInput()
        {
            Console.Write("Enter a reference id:");
              string referenceInput = Console.ReadLine();
            return referenceInput;
        }

        static void displayLoanInfo(int noOfLoans, int amount, int duration, double interestRate, double penaltyValue)
        {
            Console.WriteLine($"No of loans: {noOfLoans}");
            Console.WriteLine($"Amount: {amount} days");
            Console.WriteLine($"Duration: {duration}");
            Console.WriteLine($"Interest Rate: {interestRate}%");
            Console.WriteLine($"Due Date Penalty: {penaltyValue} Php");
        }
    }
}
    