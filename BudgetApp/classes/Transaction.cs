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
            Dictionary<int, Transaction> selectedCategoryTransactions = new();
            var selectedCategory = categoriesList[selectedCategoryID];
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (selectedCategory.CategoryID.Equals(transaction.Value.TransactionCategory.CategoryID))
                {
                    selectedCategoryTransactions.Add(transaction.Key, transaction.Value);
                }
            }
            ManageTransactions(selectedCategoryTransactions, categoriesList, usersList);
            return selectedCategoryTransactions;
        }

        public static Dictionary<int, Transaction> GetTransactionByUser(int selectedUserID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Dictionary<int, Transaction> selectedUserTransactions = new();
            var selectedUser = usersList[selectedUserID];
            foreach (KeyValuePair<int, Transaction> transaction in transactionsList)
            {
                if (selectedUser.UserID.Equals(transaction.Value.TransactionUser.UserID))
                {
                    selectedUserTransactions.Add(transaction.Key, transaction.Value);
                }
            }
            ManageTransactions(selectedUserTransactions, categoriesList, usersList);
            return selectedUserTransactions;
        }

        public static void AddTransactionReworked(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
            Console.Clear();

            AnsiConsole.Write(new Rule("[yellow]Dodaj transakcję[/]"));

            int transactionID = transactionsList.Count == 0 ? 1 : (transactionsList.Keys.Max() + 1);

            var categoriesPrompt = new SelectionPrompt<string>()
                .PageSize(10)
                .Title("Wybierz [green]kategorię[/] transakcji \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                .MoreChoicesText("[grey](Przesuwaj w górę i w dół, aby przełączać pomiędzy kategoriami)[/]");
            foreach (KeyValuePair<int, Category> category in categoriesList)
            {
                categoriesPrompt.AddChoices(category.Value.CategoryName.ToString());
            }
            string selectedCategoryName = AnsiConsole.Prompt(categoriesPrompt);
            AnsiConsole.MarkupLine("Wybrałeś kategorię: [yellow]{0}[/]", selectedCategoryName);
            int selectedCategoryID = categoriesList.FirstOrDefault(category => category.Value.CategoryName == selectedCategoryName).Key;

            double transactionAmount = AnsiConsole.Prompt(
                new TextPrompt<double>("Podaj [green]kwotę[/] transakcji: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Nieprawidłowa kwota transakcji[/]")
                    .Validate(age =>
                    {
                        return age switch
                        {
                            <= 0 => ValidationResult.Error("[red]Kwota ma być wartością bezwzględną! Program sam rozpozna wartości ujemne po kategorii[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            string description = AnsiConsole.Ask("Wprowadź [green]opis transakcji[/] (pole opcjonalne): ", " ");

            var usersPrompt = new SelectionPrompt<string>()
                .PageSize(10)
                .Title("Wybierz [green]użytkownika[/] \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                .MoreChoicesText("[grey](Przesuwaj w górę i w dół, aby przełączać pomiędzy użytkownikami)[/]");
            foreach (KeyValuePair<int, User> user in usersList)
            {
                usersPrompt.AddChoices(user.Value.UserFirstName);
            }
            string selectedUserName = AnsiConsole.Prompt(usersPrompt);
            AnsiConsole.MarkupLine("Wybrałeś użytkownika: [yellow]{0}[/]", selectedUserName);
            int selectedUserID = usersList.FirstOrDefault(user => user.Value.UserFirstName == selectedUserName).Key;

            DateTimeOffset date = GetConsoleInput.ChooseDateOfTransaction();

            Transaction addingTransaction = new(transactionID, categoriesList[selectedCategoryID], transactionAmount, description, usersList[selectedUserID], date);

            transactionsList.Add(transactionID, addingTransaction);

            AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

            AnsiConsole.Write(new Markup("\n [bold darkorange]Transakcja została pomyślnie dodana! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));                       

            Console.ReadKey();            
            Console.Clear();
        }

        public static void EditTransactionReworked(int selectedTransactionID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList)
        {
           Console.Clear();

           Dictionary<ConsoleKey, string> editOptions = new()
                    {
                        { ConsoleKey.Z, "Edycja" },
                        { ConsoleKey.X, "Usuwanie" },
                        { ConsoleKey.C, "Powrót do menu" },
                    };

           ConsoleKey selector;

           var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($" \t\t\t\t Co zamierzasz zrobić z wybraną transakcją?")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Przesuwaj w górę i w dół, a wybraną opcję zatwierdź klawiszem ENTER)[/]")
                    .AddChoices(editOptions.Values)
                    );

            AnsiConsole.WriteLine($"Wybrałeś opcję: {selectedOption}");

            selector = editOptions.FirstOrDefault(option => option.Value == selectedOption).Key;

            switch (selector)
            {
                case ConsoleKey.Z:
                    var oldTransaction = transactionsList[selectedTransactionID];

                    AnsiConsole.Write(new Rule("[yellow]Edytuj transakcję[/]"));

                    var categoriesPrompt = new SelectionPrompt<string>()
                        .PageSize(10)
                        .Title("Wybierz [green]kategorię[/] transakcji \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                        .MoreChoicesText("[grey](Przesuwaj w górę i w dół, aby przełączać pomiędzy kategoriami)[/]");
                    foreach (KeyValuePair<int, Category> category in categoriesList)
                    {
                        if (category.Value.CategoryName == transactionsList[selectedTransactionID].TransactionCategory.CategoryName)
                        {
                            categoriesPrompt.AddChoice($"{category.Value.CategoryName} (aktualna)");
                        } else
                        {
                            categoriesPrompt.AddChoice(category.Value.CategoryName.ToString());
                        }
                    }

                    string selectedCategoryName = AnsiConsole.Prompt(categoriesPrompt);
                    AnsiConsole.MarkupLine("Wybrałeś kategorię: [yellow]{0}[/]", selectedCategoryName);
                    int selectedCategoryID = categoriesList.FirstOrDefault(category => category.Value.CategoryName == selectedCategoryName).Key;

                    double transactionAmount = AnsiConsole.Prompt(
                        new TextPrompt<double>($"Podaj [green]kwotę[/] transakcji (aktualna: {transactionsList[selectedTransactionID].TransactionAmount}): ")
                            .PromptStyle("green")
                            .ValidationErrorMessage("[red]Nieprawidłowa kwota transakcji[/]")
                            .Validate(age =>
                            {
                                return age switch
                                {
                                    <= 0 => ValidationResult.Error("[red]Kwota ma być wartością bezwzględną! Program sam rozpozna wartości ujemne po kategorii[/]"),
                                    _ => ValidationResult.Success(),
                                };
                            }));

                    string description = AnsiConsole.Ask("Wprowadź [green]opis transakcji[/] (pole opcjonalne): ", transactionsList[selectedTransactionID].TransactionDescription);

                    var usersPrompt = new SelectionPrompt<string>()
                        .PageSize(10)
                        .Title("Wybierz [green]użytkownika[/] \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                        .MoreChoicesText("[grey](Przesuwaj w górę i w dół, aby przełączać pomiędzy użytkownikami)[/]");
                    foreach (KeyValuePair<int, User> user in usersList)
                    {
                        if (user.Value.UserFirstName == transactionsList[selectedTransactionID].TransactionUser.UserFirstName)
                        {
                            usersPrompt.AddChoice($"{user.Value.UserFirstName} (aktualny)");
                        }
                        else
                        {
                            usersPrompt.AddChoice(user.Value.UserFirstName.ToString());
                        }
                    }
                    string selectedUserName = AnsiConsole.Prompt(usersPrompt);
                    AnsiConsole.MarkupLine("Wybrałeś użytkownika: [yellow]{0}[/]", selectedUserName);
                    int selectedUserID = usersList.FirstOrDefault(user => user.Value.UserFirstName == selectedUserName).Key;

                    Console.Write($"Zmienić datę tej transakcji? {oldTransaction.TransactionDate:dd-MM-yyyy} (t/n): ");
                    DateTimeOffset date;
                    if (Console.ReadLine().ToUpper().Equals("T"))
                    {
                        date = GetConsoleInput.ChooseDateOfTransaction();
                    } else
                    {
                        date = oldTransaction.TransactionDate;
                    }

                    AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

                    AnsiConsole.Write(new Markup("\n [bold darkorange]Transakcja została pomyślnie zedytowana! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));
                   
                    Transaction newTransaction = new(selectedTransactionID, categoriesList[selectedCategoryID], transactionAmount, description, usersList[selectedUserID], date);

                    transactionsList[selectedTransactionID] = newTransaction;

                    Console.ReadKey();
                    Console.Clear();

                    break;

                case ConsoleKey.X:
                    AnsiConsole.Write(new Rule("[yellow]Usuń transakcję[/]"));

                    if (AnsiConsole.Confirm("Jesteś pewny, że chcesz usunąć tę transakcję?"))
                    {
                        transactionsList.Remove(selectedTransactionID);
                        AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

                        AnsiConsole.Write(new Markup("\n [bold darkorange]Transakcja została pomyślnie usunięta! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));
                        Console.ReadKey();
                    }
                    break;

                case ConsoleKey.C:
                    Console.WriteLine("Powrót do menu");
                    return;

                default:
                    Console.WriteLine("Nieprawidłowy wybór");
                    return;
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
                .AddColumn(new TableColumn("[darkorange][b]ID[/][/]").Footer("[darkorange][b]ID[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Kategoria[/][/]").Footer("[darkorange][b]Kategoria[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Wartość[/][/]").Footer("[darkorange][b]Wartość[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Opis[/][/]").Footer("[darkorange][b]Opis[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Domownik[/][/]").Footer("[darkorange][b]Domownik[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Data[/][/]").Footer("[darkorange][b]Data[/][/]").Centered());


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

            AnsiConsole.Write(new Rule("[yellow]Opcje[/]").LeftAligned());

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
                    Console.WriteLine("Brak transakcji zapisanej pod wybraną pozycją!");
                    break;

                case ConsoleKey.F:
                    var usersPrompt = new SelectionPrompt<string>()
                        .PageSize(10)
                        .Title("Wybierz [green]użytkownika[/] ([grey]Przesuwaj w górę i w dół, aby przełączać pomiędzy użytkownikami. Naciśnij [green]ENTER[/] do zatwierdzenia[/])")
                        .MoreChoicesText("[grey](Przesuwaj w górę i w dół, aby przełączać pomiędzy użytkownikami)[/]");
                    foreach (KeyValuePair<int, User> user in usersList)
                    {
                        usersPrompt.AddChoices(user.Value.UserFirstName);
                    }
                    string selectedUserName = AnsiConsole.Prompt(usersPrompt);
                    AnsiConsole.MarkupLine("Wybrałeś użytkownika: [yellow]{0}[/]", selectedUserName);
                    int selectedUserID = usersList.FirstOrDefault(user => user.Value.UserFirstName == selectedUserName).Key;

                    GetTransactionByUser(selectedUserID, transactionsList, categoriesList, usersList);
                    break;

                case ConsoleKey.C:
                    var categoriesPrompt = new SelectionPrompt<string>()
                        .PageSize(10)
                        .Title("Wybierz [green]kategorię[/] transakcji ([grey]Przesuwaj w górę i w dół, aby przełączać pomiędzy użytkownikami. Naciśnij [green]ENTER[/] do zatwierdzenia[/])")
                        .MoreChoicesText("[grey](Przesuwaj w górę i w dół, aby przełączać pomiędzy kategoriami)[/]");
                    foreach (KeyValuePair<int, Category> category in categoriesList)
                    {
                        categoriesPrompt.AddChoices(category.Value.CategoryName.ToString());
                    }
                    string selectedCategoryName = AnsiConsole.Prompt(categoriesPrompt);
                    AnsiConsole.MarkupLine("Wybrałeś kategorię: [yellow]{0}[/]", selectedCategoryName);
                    int selectedCategoryID = categoriesList.FirstOrDefault(category => category.Value.CategoryName == selectedCategoryName).Key;

                    GetTransactionByCategory(selectedCategoryID, transactionsList, categoriesList, usersList);
                    break;

                case ConsoleKey.U:
                    Console.Clear();
                    return;

                default:
                    Console.Clear();
                    return;
            }
        }
    }
}
