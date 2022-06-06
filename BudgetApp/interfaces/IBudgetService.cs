using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface IBudgetService
    {
        static Dictionary<string, string> fileNames;
        static Dictionary<int, Transaction> LoadTransactionList(string fileName) => throw new NotImplementedException($"{fileName}");
        static void SaveTransactionList(Dictionary<int, Transaction> transactionsList, string fileName) => throw new NotImplementedException($"{transactionsList}, {fileName}");
        static List<User> LoadUserList(string fileName) => throw new NotImplementedException($"{fileName}");       
        static void SaveUserList(List<User> usersList, string fileName) => throw new NotImplementedException($"{usersList}, {fileName}");
    }
}
