using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface IMenu
    {
        bool IsProgramOpen { get; set; }
        Dictionary<string,string> ProgramOptions { get; }
        void ShowUsersList(Dictionary<int, User> usersList);
        void AddTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user);
        void EditTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user);
        void ShowTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user);
        void HandleMenu(Dictionary<int, User> usersList, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user);
        void ManageProgramWorking();
    }
}
