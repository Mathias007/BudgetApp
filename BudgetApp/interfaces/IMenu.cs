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
        void ShowUsersList();
        void ShowCategoriesList();
        void AddTransactionReworked();
        void EditTransactionReworked(int selectedTransactionID);
        void PrintTransactionList();
        void HandleMenu(Dictionary<int, User> usersList, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user);
        void ManageProgramWorking();
    }
}
