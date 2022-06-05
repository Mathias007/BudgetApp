using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApp
{
    class Program
    {
        public static async Task Main()
        {
            Dictionary<string, string> fileNames = new()
            {
                { "Transactions", "TransactionsList.json" },
                { "Users", "UserList.json" },
                { "Categories", "CategoriesList.json" },
            };

            Budget budget = new();

            Transaction firstTransaction = new(0,
                 new Category("income", "transaction"),
                 2137.5,
                 "Testowa transakcja",
                 new User("Jan", "Kowalski", true, true),
                 DateTime.Parse("2019-08-01"));

            Dictionary<int, Transaction> dictionary = new();

            dictionary.Add(firstTransaction.TransactionID, firstTransaction);
            Budget.SaveTransactionList(dictionary, fileNames["Transactions"]);
            Dictionary<int, Transaction> a = Budget.LoadTransactionList(fileNames["Transactions"]);
            a[0].PrintProperties();
        }
    }
}
