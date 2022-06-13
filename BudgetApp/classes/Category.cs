using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _isActive = true; //nie ma żadnego powodu żeby kategoria którą właśnie dodaliśmy była nieaktywna
        }
        public static Category createCategory(Dictionary<int,Category> categoryList)
        {
            int newCategoryID = categoryList.Count == 0 ? 1 : categoryList.Keys.Max() + 1;
            string incomeOrExpense = "";
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
            return new Category(newCategoryID, incomeOrExpense, categoryName);
        }
        public static Category editCategory(Category categoryToBeEdited)
        {
            Console.Clear();
            Console.WriteLine($"Wpisz nową nazwę kategorii, zostaw puste żeby pominąć({categoryToBeEdited.CategoryName}): ");
            string newCategoryName = Console.ReadLine();
            categoryToBeEdited.CategoryName = String.IsNullOrWhiteSpace(newCategoryName) ? categoryToBeEdited.CategoryName : newCategoryName;
            Console.Clear();
            Console.WriteLine($"Kategoria jest aktywna({categoryToBeEdited.IsActive})? (t/n), zostaw puste żeby nie zmieniać ");
            string newActiveStatus = Console.ReadLine().ToUpper();
            if (newActiveStatus.Equals("T"))
                categoryToBeEdited.IsActive = true;
            else if (newActiveStatus.Equals("N"))
                categoryToBeEdited.IsActive = false;
            return categoryToBeEdited;
        }
    }
}
