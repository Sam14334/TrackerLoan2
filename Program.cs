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
            //IDataService dataService = new DataService.DataService(); //in-memory data saving

            


            short dataOption;
            Console.WriteLine("============= Data Saving =============");
            Console.WriteLine("Select a number for your desired data saving method");
            Console.WriteLine("[1] In-memory");
            Console.WriteLine("[2] Json file");
            Console.WriteLine("[3] Sql table");
            Console.WriteLine("[4] Exit\n");
            dataOption = Convert.ToInt16(Console.ReadLine());

            AppService.AppService appService = new AppService.AppService(dataOption);
             


            short menuOption;
            do
            {
                Console.WriteLine("============= Loan Tracker =============");
                Console.WriteLine("Select a number for your desired action");
                Console.WriteLine("[1] Track Loan");
                Console.WriteLine("[2] Add Account ");
                Console.WriteLine("[3] Reset Accounts "); 
                Console.WriteLine("[4] Update Account "); 
                Console.WriteLine("[5] Delete Account "); 
                Console.WriteLine("[10] Display Accounts(for testing)"); 
                Console.WriteLine("[6] Exit\n");

                menuOption = Convert.ToInt16(Console.ReadLine());

                if (menuOption == 1)
                {

                    string refInput = getRefInput();

                    Account account = appService.GetAccountByReference(refInput);// if input matches any accountReference in dummyAccounts, it will return the first match, otherwise its null.
                   

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
                else if (menuOption == 2)
                {
                    string accountReference;
                    int daysPassed, duration, interestRate, penaltyRate;
                    double amount ;

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

                    if (appService.RegisterAccount(newAccount))
                    {
                        Console.WriteLine("Account Added!\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Parameters/Account already exists\n");
                    }
                 
                }else if (menuOption == 3)
                {
                    if (appService.ResetAccounts())
                    {
                        Console.WriteLine("Accounts Reset!\n");
                    }
                    else
                    {
                        Console.WriteLine("Unexpected Problem occured\n");
                    }
                }else if (menuOption == 4)
                {
                    string changeRefInput = getChangeRefInput();

                    Account account = appService.GetAccountByReference(changeRefInput);
                    //account is a reference to the account in the list, any changes to reference update the original
                    //points to the memory location of the account in list
                    if (account == null)
                    {
                        Console.WriteLine("Account not found!\n");
                    }
                    else
                    {
                        string accountReference;
                        int daysPassed, duration, interestRate, penaltyRate;
                        double amount;

                        Console.WriteLine($"============= Edit Account {account.accountReference} =============");
                        Console.Write("Enter new Reference ID: ");
                        accountReference = Console.ReadLine();
                        Console.Write("Enter new Amount (Php): ");
                        amount = double.Parse(Console.ReadLine());
                        Console.Write("Enter new DaysPassed since loan: ");
                        daysPassed = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Duration of loan: ");
                        duration = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Interest Rate (percentage): ");
                        interestRate = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Penalty Rate (percentage): ");
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

                        if (appService.UpdateAccount(account, newAccount))
                        {
                            Console.WriteLine("Account Updated!\n");
                        }
                        else
                        {
                            Console.WriteLine("Invalid reference!\n");

                        } 
                    }
                        
                }
                else if(menuOption == 5)
                {
                    string deleteReferenceInput = getDeleteRefInput();
                    Account account = appService.GetAccountByReference(deleteReferenceInput);

                    if (appService.DeleteAccount(account))
                    {
                        Console.WriteLine("Account Deleted!\n");
                    }
                    else
                    {
                        Console.WriteLine("Account not found!\n");
                    }
                        ;
                }
                else if (menuOption == 10)
                {
                    displayAllAccounts(appService.GetAllAccounts());
                }
                else if (menuOption == 6)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid number option\n");
                }
            } while (menuOption != 6);
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
        static void displayAllAccounts(List<Account> account)
        {
            if (!account.Any())
            {
                Console.WriteLine("No accounts to display!");
                return;
            }
            foreach (Account item in account)
            {
                Console.WriteLine($"Reference ID:{item.accountReference}\n");
            }
        }
        static string getRefInput()
        {
            Console.Write("Enter your reference id:");
            string referenceInput = Console.ReadLine();
            return referenceInput;
        }
      
        static string getChangeRefInput()
        {
            Console.Write("Enter account reference to be changed:");
            string changeReferenceInput = Console.ReadLine();
            return changeReferenceInput;
        }
        static string getDeleteRefInput()
        {
            Console.Write("Enter account reference to be deleted:");
            string deleteReferenceInput = Console.ReadLine();
            return deleteReferenceInput;
        }


    }
}
