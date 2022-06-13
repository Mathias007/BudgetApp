using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Category : ICategory
    {
        private int _id;
        private string _type;
        private string _name;
        private bool _isActive;

        public int CategoryID { get => _id; set => _id = value; }
        public string CategoryType { get => _type; set => _type = value; }
        public string CategoryName { get => _name; set => _name = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public Category(int id, string type, string name)
        {
            _id = id;
            _type = type;
            _name = name;
            _isActive = true;
        }
        public static void PrintCategories(bool onlyActive, Dictionary<int, Category> categoriesList)
        {
            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                if (!onlyActive || record.Value.IsActive)
                {
                    if (record.Value.CategoryType == "expense") Console.ForegroundColor = ConsoleColor.Red;
                    else if (record.Value.CategoryType == "income") Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine(
                            $" + {record.Key}: " +
                            $"{record.Value.CategoryName} ({record.Value.CategoryType})");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public static void ManageCategories(Dictionary<int, Category> categoriesList)
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich kategorii:");
            Console.ForegroundColor = ConsoleColor.Yellow;


            Console.WriteLine(" + [0]: dodaj kategorię");
            PrintCategories(false, categoriesList);

            Console.WriteLine(" \n -> Wybierz 0, aby dodać nową kategorię. \n -> Jeżeli chcesz zmodyfikować dane istniejącej kategorii, wypisz jego numer ID. \n -> Aby wrócić do głównego menu, naciśnij ENTER, pozostawiając pole puste.");
            int consoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
            Console.Clear();
            if (consoleID == -1)
            {
                Console.Clear();
                return;
            }
            else if (consoleID == 0)
            {
                int newCategoryID = categoriesList.Count == 0 ? 1 : categoriesList.Keys.Max() + 1;
                string incomeOrExpense;

                while (true)
                {
                    Console.WriteLine("Dochód czy Wydatek? (d/w): ");
                    incomeOrExpense = Console.ReadLine().ToUpper();
                    if (incomeOrExpense.Equals("D"))
                    {
                        incomeOrExpense = "income";
                        break;
                    }
                    else if (incomeOrExpense.Equals("W"))
                    {
                        incomeOrExpense = "expense";
                        break;
                    }
                    else
                    {
                        Console.WriteLine("nieprawidłowy wybór");
                    }
                }
                Console.Clear();
                Console.WriteLine("Nazwa kategorii: ");
                string categoryName = Console.ReadLine();
                Category addingCategory = new(newCategoryID, incomeOrExpense, categoryName);
                categoriesList.Add(addingCategory.CategoryID, addingCategory);
                return;
            }

            Console.Clear();

            Console.WriteLine($"Wpisz nową nazwę kategorii, zostaw puste żeby pominąć({categoriesList[consoleID].CategoryName}): ");
            string newCategoryName = Console.ReadLine();
            categoriesList[consoleID].CategoryName = String.IsNullOrWhiteSpace(newCategoryName) ? categoriesList[consoleID].CategoryName : newCategoryName;
            Console.Clear();

            Console.WriteLine($"Kategoria jest aktywna({categoriesList[consoleID].IsActive})? (t/n), zostaw puste żeby nie zmieniać ");
            string newActiveStatus = Console.ReadLine().ToUpper();

            if (newActiveStatus.Equals("T"))
                categoriesList[consoleID].IsActive = true;
            else if (newActiveStatus.Equals("N"))
                categoriesList[consoleID].IsActive = false;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }   
    }
}
