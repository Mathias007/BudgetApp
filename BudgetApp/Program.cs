using System;
using System.Collections.Generic;

namespace BudgetApp
{
    class Program
    {
        public static void Main()
        {

            Budget budget = new();

            Menu menu = new();

            Category firstCategory = new(1, "income", "Wynagrodzenie");
            User firstUser = new(1, "Jan", "Kowalski", true, true);

            Transaction firstTransaction = new(
                 1,
                 firstCategory,
                 2137.5,
                 "Testowa transakcja",
                 firstUser,
                 DateTime.Parse("2019-08-01")
            );

            Dictionary<int, Transaction> transactions = new();
            Dictionary<int, Category> categories = new();
            Dictionary<int, User> users = new();

            categories.Add(firstCategory.CategoryID, firstCategory); 
            users.Add(firstUser.UserID, firstUser);
            transactions.Add(firstTransaction.TransactionID, firstTransaction);

            //menu.HandleMenu(users, transactions, categories, firstUser);

            // budget.EstablishBudgetStructure();

            testUserlist();
        }
        private static void testUserlist()
        {
            Budget budget = new();

            Menu menu = new();

            Category firstCategory = new(1, "income", "Wynagrodzenie");
            Category category1 = new(2, "income", "dochód z wynajętego mieszkania");
            Category category2 = new(3, "expense", "opłata za prąd");
            Category category3 = new(4, "expense", "zakupy spożywcze");
            User firstUser = new(1, "Jan", "Kowalski", true, true);

            Transaction firstTransaction = new(
                 0,
                 firstCategory,
                 2137.5,
                 "Testowa transakcja",
                 firstUser,
                 DateTime.Parse("2019-08-01")
            );

            Dictionary<int, Transaction> transactions = new();
            transactions.Add(firstTransaction.TransactionID,firstTransaction);

            Dictionary<int, Category> categories = new();
            categories.Add(firstCategory.CategoryID, firstCategory);
            categories.Add(category1.CategoryID, category1);
            categories.Add(category2.CategoryID, category2);
            categories.Add(category3.CategoryID, category3);


            var user1 = new User(1, "Monika", "Jeden", true, true);
            var user2 = new User(2, "Janusz", "Dwa", true, true);
            var user3 = new User(3, "Bogdan", "Trzy", true, true);

            Dictionary<int, User> usersTest = new();
            usersTest.Add(user1.UserID, user1);
            usersTest.Add(user2.UserID, user2);
            usersTest.Add(user3.UserID, user3);

            menu.HandleMenu(firstUser);
        }
        public static DateTimeOffset ChooseDateOfTransaction()
        {
            Console.WriteLine("Jeśli transakcja jest z dzisiaj zostaw puste pole, w innym wypadku wprowadź datę w formacie DD-MM-RRRR");
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    return DateTimeOffset.Now;
                }
                DateTimeOffset returnDate = DateTimeOffset.MinValue;
                if (DateTimeOffset.TryParseExact(consoleInput, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out returnDate))
                {
                    return returnDate;
                }
                Console.WriteLine($"Nieprawidłowy format daty! ma być w formacie DD-MM-RRRR, przykład - dzisiaj jest {DateTimeOffset.Now.ToString("dd-MM-yyyy")}");
            }
        }

    }
}
