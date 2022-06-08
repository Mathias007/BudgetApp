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
        public bool CategoryIsActive { get => _isActive; set => _isActive = value; }

        public Category(int id, string type, string name)
        {
            _id = id;
            _type = type;
            _name = name;
            _isActive = true; //nie ma żadnego powodu żeby kategoria którą właśnie dodaliśmy była nieaktywna
        }
        public static Category addCategory(int maxId)
        {
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
            Console.WriteLine("Nazwa kategorii: ");
            string categoryName = Console.ReadLine();
            return new Category(maxId + 1, incomeOrExpense, categoryName);
        }
    }
}
