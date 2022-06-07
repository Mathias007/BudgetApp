using System;
using System.Collections.Generic;

namespace BudgetApp
{
    class Program
    {
        public static void Main()
        {
            Budget budget = new(Budget.LoadTransactionList(Budget.fileNames["Transactions"]));
            budget.CalculateBalance();

            Menu menu = new(budget);

            Category firstCategory = new(0, "income", "Wynagrodzenie");
            User firstUser = new(0, "Jan", "Kowalski", true, true);

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

            // budget.EstablishBudgetStructure();

            menu.HandleMenu(users, transactions, categories, firstUser);
        }
    }
}
