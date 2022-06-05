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
            Menu menu = new();

            Category firstCategory = new("income", "Wynagrodzenie");
            User firstUser = new("Jan", "Kowalski", true, true);

            Transaction firstTransaction = new(
                 0,
                 firstCategory,
                 2137.5,
                 "Testowa transakcja",
                 firstUser,
                 DateTime.Parse("2019-08-01")
            );

            Dictionary<int, Transaction> transactions = new();
            Dictionary<int, Category> categories = new();
            Dictionary<int, User> users = new();

            categories.Add(0, firstCategory);
            users.Add(0, firstUser);
            transactions.Add(firstTransaction.TransactionID, firstTransaction);

            menu.HandleMenu(users, transactions, categories, firstUser);

            Budget.SaveTransactionList(transactions, fileNames["Transactions"]);
            Dictionary<int, Transaction> a = Budget.LoadTransactionList(fileNames["Transactions"]);
            a[1].PrintProperties();
        }
    }
}
