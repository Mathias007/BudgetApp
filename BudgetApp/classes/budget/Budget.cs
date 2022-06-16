using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Budget : BudgetService, IBudget
    {
        private double _balance = 0;
        private Dictionary<string, (double, double)> _budgetStructure;

        internal static Dictionary<int, Transaction> transactionsList;
        internal static Dictionary<int, User> usersList;
        internal static Dictionary<int, Category> categoriesList;

        public double BudgetBalance { get => _balance; set => _balance = value; }
        public Dictionary<string, (double CategoryAmount, double CategoryPercentage)> BudgetStructure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Dictionary<int, Transaction> TransactionsList { get => transactionsList; set => transactionsList = value; }
        public Dictionary<int, Category> CategoriesList { get => categoriesList; set => categoriesList = value; }
        public Dictionary<int, User> UsersList { get => usersList; set => usersList = value; }

        public Budget()
        {
            transactionsList = LoadTransactionList(fileNames["Transactions"]);
            usersList = LoadUserList(fileNames["Users"]);
            categoriesList = LoadCategoryList(fileNames["Categories"]);
        }

        private void CalculateBalance()
        {
            foreach (KeyValuePair<int, Transaction> record in transactionsList) _balance += record.Value.TransactionAmount;

            Console.WriteLine($"Stan konta: {_balance}");
            _balance = 0;
        }

        private void EstablishBudgetStructure()
        {
            // Krok 1. Zsumować kwoty w poszczególnych kategoriach.
            //         - dla każdej kategorii w liście kategorii przefiltrować BudgetData,
            //         - z pozyskanych transakcji zsumować wartości z Amount.
            // Krok 2. Wyliczyć procentową wartość z wykorzystaniem BudgetBalance.
            // Krok 3. Wygenerować słownik:
            //         - nazwy transakcji jako klucze,
            //         - krotka zawierająca sumy i procenty z ww. kroków jako wartości.
            // Krok 4. Wyświetlić uporządkowaną tabelę w konsoli.

            _budgetStructure = new Dictionary<string, (double CategoryAmount, double CategoryPercentage)>()
            {
                ["Wynagrodzenie"] = (4, 10),
                ["Zakupy"] = (10, 20),
                ["Media"] = (0, 23)
            };

            AnsiConsole.Write(new BarChart()
                .Width(60)
                .Label("[green bold underline]Number of fruits[/]")
                .CenterLabel()
                .AddItem("Apple", 12, Color.Yellow)
                .AddItem("Orange", 54, Color.Green)
                .AddItem("Banana", 33, Color.Red));

            foreach (KeyValuePair<string, (double amount, double percentage)> record in _budgetStructure)
            {
                Console.WriteLine($" + {record.Key}: {record.Value.amount} PLN ({record.Value.percentage}%)");
            }
        }

        public void ManageBudgetSummary()
        {
            Console.Clear();

            CalculateBalance();
            EstablishBudgetStructure();

            Console.ReadKey();
        }
    }
}