using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace BudgetApp
{
    public class Transaction : BudgetElement, ITransaction
    {
        private Category _category;
        private double _amount;
        private string _description;
        private User _user;
        private DateTimeOffset _date;

        public int TransactionID { get => _id; set => _id = value; }
        public Category TransactionCategory { get => _category; set => _category = value; }
        public double TransactionAmount { get => _amount; set => _amount = value; }
        public string TransactionDescription { get => _description; set => _description = value; }
        public User TransactionUser { get => _user; set => _user = value; }
        public DateTimeOffset TransactionDate { get => _date; set => _date = value; }

        public Transaction(int id, Category category, double amount, string description, User user, DateTimeOffset date)
        {
            _id = id;
            _category = category;
            _amount = amount;
            _description = description;
            _user = user;
            _date = date;
        }

        public override void PrintProperties()
        {
            Console.WriteLine($"id: {_id} \n" +
                $"categoryName: {_category.CategoryName} \n" +
                $"categoryType: {_category.CategoryType} \n" +
                $"amount: {_amount} \n" +
                $"description: {_description} \n" +
                $"user: {_user.UserFirstName} {_user.UserLastName} \n" +
                $"date: {_date}");
        }

        public static Dictionary<int, Transaction> GetTransactionByCategory(int selectedCategoryID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Dictionary<int, Transaction> selectedCategoryTransaciton = new();
            var selectedCategory = categoriesList[selectedCategoryID]; //bez walidacji, walidacja bedzie tam gdzie będzie ta metoda jest wywoływana
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (selectedCategory.Equals(transaction.Value.TransactionCategory))
                {
                    selectedCategoryTransaciton.Add(transaction.Key, transaction.Value);
                }
            }
            Transaction.ManageTransactions(selectedCategoryTransaciton, categoriesList, usersList);
            return selectedCategoryTransaciton;
        }

        public static Dictionary<int, Transaction> GetTransactionByUser(int selectedUserID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Dictionary<int, Transaction> selectedUserTransaciton = new();
            var selectedUser = usersList[selectedUserID]; //bez walidacji, walidacja bedzie tam gdzie będzie ta metoda jest wywoływana
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (selectedUser.Equals(transaction.Value.TransactionUser))
                {
                    selectedUserTransaciton.Add(transaction.Key, transaction.Value);
                }
            }
            Transaction.ManageTransactions(selectedUserTransaciton, categoriesList, usersList);
            return selectedUserTransaciton;
        }

        public static void AddTransactionReworked(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {

            Console.Clear();
            int transactionID = transactionsList.Count == 0 ? 1 : (transactionsList.Keys.Max() + 1);

            Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer: ");
            Category.PrintCategories(true, categoriesList);
            int selectedCategoryID = GetConsoleInput<Category>.GetUserInputID(categoriesList, true);
            Console.Clear();
            double transactionAmmount = GetConsoleInput.UserInputTransactionAmmount(false);
            Console.Clear();
            Console.Write("Wprowadź opis transakcji (pole opcjonalne): ");
            string description = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Do którego domownika należy ta transakcja?");
            User.PrintUsers(false, usersList);
            int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);
            Console.Clear();
            DateTimeOffset date = GetConsoleInput.ChooseDateOfTransaction();

            transactionsList.Add(transactionID, new Transaction(transactionID, categoriesList[selectedCategoryID], transactionAmmount, description, usersList[selectedUserID], date));
            Console.Clear();
        }

        public static void EditTransactionReworked(int selectedTransactionID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Console.Clear();
            Console.WriteLine("Co zamierzasz zrobić z wybraną transakcją? [e] - edycja, [d] - usuwanie, [jakikolwiek inny klawisz] - wróć do menu");
            transactionsList[selectedTransactionID].PrintProperties();
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.E:
                    var oldTransaction = transactionsList[selectedTransactionID];

                    Console.WriteLine($"Wybierz nową kategorie ({oldTransaction.TransactionCategory.CategoryName}), zostaw puste żeby nie zmieniać");
                    Category.PrintCategories(true, categoriesList);
                    int selectedNewCategoryID = GetConsoleInput<Category>.GetUserInputID(categoriesList, true);
                    transactionsList[selectedTransactionID].TransactionCategory = selectedNewCategoryID == -1 ? transactionsList[selectedTransactionID].TransactionCategory : categoriesList[selectedNewCategoryID];
                    Console.Clear();
                    Console.WriteLine($"Wpisz nową kwotę ({oldTransaction.TransactionAmount}), zostaw puste żeby nie zmieniać");
                    double newAmmount = GetConsoleInput.UserInputTransactionAmmount(true);
                    transactionsList[selectedTransactionID].TransactionAmount = newAmmount == -1 ? transactionsList[selectedTransactionID].TransactionAmount : newAmmount;
                    Console.Clear();
                    Console.WriteLine($"Wpisz nowy opis transakcji {oldTransaction.TransactionDescription}, zostaw puste żeby nie zmieniać"); //ogarnąć żeby wyświetlało to estetycznie
                    string newDescription = Console.ReadLine();
                    transactionsList[selectedTransactionID].TransactionDescription = string.IsNullOrWhiteSpace(newDescription) ? transactionsList[selectedTransactionID].TransactionDescription : newDescription;
                    Console.Clear();
                    Console.WriteLine($"Przypisz tą transakcje do innego domownika ({oldTransaction.TransactionUser.UserFirstName} {oldTransaction.TransactionUser.UserLastName}), zostaw puste żeby nie zmieniać");
                    User.PrintUsers(true, usersList);
                    int selectedNewUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);
                    transactionsList[selectedTransactionID].TransactionUser = selectedNewUserID == -1 ? transactionsList[selectedTransactionID].TransactionUser : usersList[selectedNewUserID];
                    Console.Clear();
                    Console.WriteLine($"Zmienić datę tej transakcji? {oldTransaction.TransactionDate.ToString("dd-MM-yyyy")} (t/n)");
                    if (Console.ReadLine().ToUpper().Equals("T"))
                    {
                        transactionsList[selectedTransactionID].TransactionDate = GetConsoleInput.ChooseDateOfTransaction();
                    }
                    break;

                case ConsoleKey.D:
                    transactionsList.Remove(selectedTransactionID);
                    Console.WriteLine("Usuwanie zakończone!");
                    break;

                default:
                    Console.WriteLine("Nieprawidłowy wybór");
                    Menu.ExitFromProgram();
                    break;
            }
        }

        public static void ManageTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Console.Clear();

            var transactionsTable = new Table();
            var font = FigletFont.Load(BudgetService.GetDatabasePath("assets/ogre.flf"));

            AnsiConsole.Write(
                     new FigletText(font, "Transakcje")
                    .Centered()
                    .Color(Color.Blue));

            transactionsTable
                .Border(TableBorder.Ascii)
                .AddColumn(new TableColumn("[darkorange][b]ID[/][/]").Footer("ID").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Kategoria[/][/]").Footer("Kategoria").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Wartość[/][/]").Footer("Wartość").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Opis[/][/]").Footer("Opis").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Domownik[/][/]").Footer("Domownik").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Data[/][/]").Footer("Data").Centered());


            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                transactionsTable.AddRow(
                    transaction.Value.TransactionID.ToString(),
                    transaction.Value.TransactionCategory.CategoryName.ToString(),
                    $"{(transaction.Value.TransactionCategory.CategoryType == "income" ? "[green]" : "[red]-")}{transaction.Value.TransactionAmount} zł[/]",
                    transaction.Value.TransactionDescription.ToString(),
                    transaction.Value.TransactionUser.UserFirstName.ToString(),
                    transaction.Value.TransactionDate.ToString()
                 );
            }

            AnsiConsole.Write(transactionsTable); 

            // Console.WriteLine("[0] - dodaj nową transakcje");
            // bool colorChanger = false;


            //foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            //{
            //    if (colorChanger)
            //    {
            //        if (transaction.Value.TransactionCategory.CategoryType.Equals("income"))
            //        {
            //            Console.ForegroundColor = ConsoleColor.DarkGreen;
            //        }
            //        if (transaction.Value.TransactionCategory.CategoryType.Equals("expense"))
            //        {
            //            Console.ForegroundColor = ConsoleColor.DarkRed;
            //        }
            //    }
            //    else if (!colorChanger)
            //    {
            //        if (transaction.Value.TransactionCategory.CategoryType.Equals("income"))
            //        {
            //            Console.ForegroundColor = ConsoleColor.Green;
            //        }
            //        if (transaction.Value.TransactionCategory.CategoryType.Equals("expense"))
            //        {
            //            Console.ForegroundColor = ConsoleColor.Red;
            //        }
            //    }
            //    Console.WriteLine($"[{transaction.Key}] : ");
            //    transaction.Value.PrintProperties();
            //    colorChanger = !colorChanger;
            // }

             Dictionary<ConsoleKey, string> transactionsOptions = new()
             {
                { ConsoleKey.W, "Dodaj nową transakcję" },
                { ConsoleKey.D, "Modyfikuj istniejącą transakcję" },
                { ConsoleKey.F, "Wyświetl transakcje wybranego domownika" },
                { ConsoleKey.C, "Wyświetl transakcje danej kategorii" },
                { ConsoleKey.U, "Wróć do menu" }
             };

            ConsoleKey selector;

            var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($" \t\t\t\t [darkorange]Co chcesz zrobić?[/]")
                .PageSize(5)
                .MoreChoicesText("[grey](Przesuwaj w górę i w dół, a wybraną opcję zatwierdź klawiszem ENTER)[/]")
                .AddChoices(transactionsOptions.Values)
        );

            AnsiConsole.WriteLine($"Wybrałeś opcję: {selectedOption}");

            selector = transactionsOptions.FirstOrDefault(option => option.Value == selectedOption).Key;

            // Console.ForegroundColor = ConsoleColor.Gray;
           // Console.WriteLine(
                //"\n -> Wybierz 0, aby dodać nową transakcję. " +
                //"\n Wybierz U, aby wyświetlić listę transakcji danego użytkownika " +
                //"\n Wybierz C, aby wyświetlić listę transakcji danej kategorii " +
                //"\n -> Jeżeli chcesz zmodyfikować dane istniejącej transakacji, wypisz jego numer ID. " +
                //"\n -> Aby wrócić do głównego menu, naciśnij ENTER, pozostawiając pole puste. ");

            switch (selector)
            {
                case ConsoleKey.W:
                    AddTransactionReworked(transactionsList, categoriesList, usersList);
                    break;

                case ConsoleKey.D:
                    Console.Write("Podaj id wybranej transakcji: ");
                    string inputID = Console.ReadLine();
                    int selectedID = -1;
                    if (int.TryParse(inputID, out selectedID) && transactionsList.ContainsKey(selectedID))
                    {
                        EditTransactionReworked(selectedID, transactionsList, categoriesList, usersList);
                        return;
                    }
                    Console.WriteLine("podanego id nie ma na liscie transakcji");
                    break;

                case ConsoleKey.F:
                    User.PrintUsers(false, usersList);
                    int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, false);
                    if (selectedUserID == -1)
                        return;
                    GetTransactionByUser(selectedUserID, transactionsList, categoriesList, usersList );
                    break;

                case ConsoleKey.C:
                    Category.PrintCategories(false, categoriesList);
                    int selectedConsoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
                    if (selectedConsoleID == -1)
                        return;
                    GetTransactionByCategory(selectedConsoleID, transactionsList, categoriesList, usersList);
                    break;

                case ConsoleKey.U:
                    Console.Clear();
                    return;

                default:
                    Console.Clear();
                    return;
            }


            //while (true)
            //{
            //    string consoleInput = Console.ReadLine();
            //    if (string.IsNullOrWhiteSpace(consoleInput))
            //    {
            //        Console.Clear();
            //        return;
            //    }
            //    if (consoleInput.Equals("0"))
            //    {
            //        AddTransactionReworked(transactionsList, categoriesList, usersList);
            //        return;
            //    }
            //    int selectedID = -1;
            //    if (int.TryParse(consoleInput, out selectedID) && transactionsList.ContainsKey(selectedID))
            //    {
            //        EditTransactionReworked(selectedID, transactionsList, categoriesList, usersList);
            //        return;
            //    }
            //    Console.WriteLine("podanego id nie ma na liscie transakcji");
            //}
        }
    }
}
