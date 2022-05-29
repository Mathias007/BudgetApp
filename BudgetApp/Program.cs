using System;
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
                 new User("Jan", "Kowalski", true),
                 DateTime.Parse("2019-08-01"));

            await firstTransaction.AddTransactionToDB(firstTransaction);

            // await budget.ShowTransactionList();

        }
    }
}
