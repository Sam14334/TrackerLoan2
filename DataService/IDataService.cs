using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    //This is an interface, an interface is like a contract of some sort that makes it so that the classes that implements it must 
    //follow its defined methods and attributes
    public interface IDataService
    {
        void populate();
        bool addAccount(Account account);

        List<Account> getAccounts();
    }
}
