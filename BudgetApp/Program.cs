using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApp
{
    class Program
    {
        public static async Task Main()
        {
            Budget budget = new();

            budget.CreateBudgetDB();

            Transaction firstTransaction = new(0,
                 new Category("income", "transaction"),
                 2137.5,
                 "Testowa transakcja",
                 new User("Jan", "Kowalski", true, true),
                 DateTime.Parse("2019-08-01"));

            Dictionary<int, Transaction> dictionary = new();

            dictionary.Add(firstTransaction.TransactionID, firstTransaction);
            JsonLoader.SaveTransactionList(dictionary);
            Dictionary<int, Transaction> a = JsonLoader.LoadTransactionList();
            a[0].PrintProperties();
        }
    }
}
