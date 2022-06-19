﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using UtilityLibraries;

namespace BudgetApp
{
    public class Budget : BudgetService, IBudget
    {
        private double _balance = 0;
        private static Dictionary<string, double> _incomeStructure = new();
        private static Dictionary<string, double> _expenseStructure = new();


        internal static Dictionary<int, Transaction> transactionsList;
        internal static Dictionary<int, User> usersList;
        internal static Dictionary<int, Category> categoriesList;

        public double BudgetBalance { get => _balance; set => _balance = value; }
        public Dictionary<string, double> IncomeStructure { get => _incomeStructure; set => _incomeStructure = value; }
        public Dictionary<string, double> ExpenseStructure { get => _expenseStructure; set => _expenseStructure = value; }


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
            _balance = 0;
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList) {
                if (transaction.Value.TransactionCategory.CategoryType == "income") _balance += transaction.Value.TransactionAmount;
                else _balance -= transaction.Value.TransactionAmount;
            }

            AnsiConsole.Markup($"Stan konta: [slowblink]{_balance} zł[/] \n");
        }

        private static void EstablishBudgetStructure()
        {
           // static int RandomizeNumber(int min, int max) => new Random().Next(min, max);
            Color[] colors = { Color.Green, Color.Yellow, Color.Red };


            foreach (KeyValuePair<int, Category> category in categoriesList)
            {
                double categorySum = 0;

                Dictionary<int, Transaction> selectedCategoryTransactions = new();
                var selectedCategory = categoriesList[category.Key];
                foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
                {
                    if (selectedCategory.CategoryID.Equals(transaction.Value.TransactionCategory.CategoryID))
                    {
                        selectedCategoryTransactions.Add(transaction.Key, transaction.Value);
                    }
                }

                foreach (KeyValuePair<int, Transaction> transaction in selectedCategoryTransactions)
                {
                    categorySum += transaction.Value.TransactionAmount;
                }

                if (category.Value.CategoryType == "income") _incomeStructure.Add(category.Value.CategoryName, categorySum);
                else _expenseStructure.Add(category.Value.CategoryName, categorySum);
            }

            AnsiConsole.Write(new Rule("[yellow]Struktura przychodów[/]"));

            var incomeChart = new BarChart()
                .Width(60)
                .CenterLabel();


                foreach (KeyValuePair<string, double> record in _incomeStructure)
                {
                    incomeChart.AddItem(record.Key.ToString(), record.Value, colors[UtilitiesLibrary.RandomizeNumber(0, colors.Length)]);
                }

            AnsiConsole.Write(incomeChart);

            AnsiConsole.Write(new Rule("[yellow]Struktura wydatków[/]"));

            var expenseChart = new BarChart()
                .Width(60)
                .CenterLabel();

                foreach (KeyValuePair<string, double> record in _expenseStructure)
                {
                    expenseChart.AddItem(record.Key.ToString(), record.Value, colors[UtilitiesLibrary.RandomizeNumber(0, colors.Length)]);
                }

            AnsiConsole.Write(expenseChart);

        }

        private static void ClearBudgetData()
        {
            _incomeStructure = new Dictionary<string, double>();
            _expenseStructure = new Dictionary<string, double>();
        }

        public void ManageBudgetSummary()
        {
            Console.Clear();

            var transactionsTable = new Table();
            var font = FigletFont.Load(UtilitiesLibrary.GetDatabasePath("assets/ogre.flf"));

            AnsiConsole.Write(
                     new FigletText(font, "Podsumowanie")
                    .Centered()
                    .Color(Color.Blue));

            CalculateBalance();
            EstablishBudgetStructure();
            ClearBudgetData();

            Console.ReadKey();
        }
    }
}