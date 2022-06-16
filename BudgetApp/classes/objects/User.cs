using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class User : BudgetElement, IUser
    {
        private string _firstName;
        private string _lastName;
        private bool _isActive;
        private bool _isAdmin;

        public int UserID { get => _id; set => _id = value; }
        public string UserFirstName { get => _firstName; set => _firstName = value; }
        public string UserLastName { get => _lastName; set => _lastName = value; }
        public bool UserIsActive { get => _isActive; set => _isActive = value; }
        public bool UserIsAdmin { get => _isAdmin; set => _isAdmin = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public User(int id, string firstName, string lastName, bool isActive = true, bool isAdmin = false)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
            _isActive = isActive;
            _isAdmin = isAdmin;
        }

        public override void PrintProperties()
        {
            Console.WriteLine($"id: {_id} \n" +
                $"firstName: {_firstName} \n" +
                $"lastName: {_lastName} \n" +
                $"isActive: {_isActive} \n" +
                $"isAdmin: {_isAdmin} \n");
        }

        public static void PrintUsers(bool onlyActive, Dictionary<int, User> usersList)
        {
            foreach (KeyValuePair<int, User> record in usersList)
            {
                if (!onlyActive || record.Value.IsActive)
                {
                    Console.WriteLine(
                        $" + [{record.Key}]: " +
                        $"{record.Value.UserFirstName} {record.Value.UserLastName} " +
                        $"{(record.Value.UserIsActive ? "AKTYWNY" : "NIEAKTYWNY")} " +
                        $"{(record.Value.UserIsAdmin ? "ADMINISTRATOR" : "USER")} ");
                }
            }
        }

        public static void AddNewUser(Dictionary<int, User> usersList)
        {
            Console.Clear();

            AnsiConsole.Write(new Rule("[yellow]Dodaj kategorię[/]"));

            int userID = usersList.Count == 0 ? 1 : (usersList.Keys.Max() + 1);

            string userFirstName = AnsiConsole.Ask<string>("Wprowadź [green]imię[/]: ");
            string userLastName = AnsiConsole.Ask<string>("Wprowadź [green]nazwisko[/]: ");

            bool userIsActive = AnsiConsole.Confirm("Czy domownik ma być aktywny?");

            var usersPrompt = new SelectionPrompt<string>()
                .PageSize(5)
                .Title("Wybierz poziom uprawnień, jakie ma posiadać tworzony w systemie domownik \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                .AddChoices(new[] { "USER", "ADMIN" });

            bool userIsAdmin = AnsiConsole.Prompt(usersPrompt) == "ADMIN";
            AnsiConsole.MarkupLine("Wybrany poziom uprawnień to: [yellow]{0}[/]", userIsAdmin ? "ADMIN" : "USER");

            User addingUser = new(userID, userFirstName, userLastName, userIsActive, userIsAdmin);

            usersList.Add(addingUser.UserID, addingUser);

            AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

            AnsiConsole.Write(new Markup("\n [bold darkorange]Użytkownik został pomyślnie dodany! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));

            Console.ReadKey();
            Console.Clear();
        }

        public static void EditExistingUser(int selectedUserID, Dictionary<int, User> usersList)
        {
            AnsiConsole.Write(new Rule("[yellow]Edytuj użytkownika[/]"));

            string userFirstName = AnsiConsole.Ask("Wprowadź [green]imię[/]: ", usersList[selectedUserID].UserFirstName);
            string userLastName = AnsiConsole.Ask("Wprowadź [green]nazwisko[/]: ", usersList[selectedUserID].UserLastName);

            bool userIsActive = AnsiConsole.Confirm("Czy domownik ma być aktywny?");

            var usersPrompt = new SelectionPrompt<string>()
                .PageSize(5)
                .Title("Wybierz poziom uprawnień, jakie ma posiadać tworzony w systemie domownik \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                .AddChoices(new[] { "USER", "ADMIN" });

            bool userIsAdmin = AnsiConsole.Prompt(usersPrompt) == "ADMIN";
            AnsiConsole.MarkupLine("Wybrany poziom uprawnień to: [yellow]{0}[/]", userIsAdmin ? "ADMIN" : "USER");

            AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

            AnsiConsole.Write(new Markup("\n [bold darkorange]Dane domownika zostały pomyślnie zedytowane! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));

            User newUser = new(selectedUserID, userFirstName, userLastName, userIsActive, userIsAdmin);

            usersList[selectedUserID] = newUser;

            Console.ReadKey();
            Console.Clear();
        }

        public static void ManageUsers(Dictionary<int, User> usersList)
        {
            Console.Clear();

            var usersTable = new Table();
            var font = FigletFont.Load(BudgetService.GetDatabasePath("assets/ogre.flf"));

            AnsiConsole.Write(
                     new FigletText(font, "Domownicy")
                    .Centered()
                    .Color(Color.Blue));

            usersTable
                .Border(TableBorder.Ascii)
                .AddColumn(new TableColumn("[darkorange][b]ID[/][/]").Footer("[darkorange][b]ID[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Imię[/][/]").Footer("[darkorange][b]Imię[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Nazwisko[/][/]").Footer("[darkorange][b]Nazwisko[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Aktywny[/][/]").Footer("[darkorange][b]Aktywny[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Poziom[/][/]").Footer("[darkorange][b]Poziom[/][/]").Centered());

            foreach (KeyValuePair<int, User> user in usersList)
            {
                usersTable.AddRow(
                    user.Value.UserID.ToString(),
                    user.Value.UserFirstName.ToString(),
                    user.Value.UserLastName.ToString(),
                    $"{(user.Value.IsActive ? "[blue]TAK[/]" : "[grey]NIE[/]")}",
                    $"{(user.Value.UserIsAdmin ? "[red]ADMIN[/]" : "[blue]USER[/]")}"
                 );
            }

            AnsiConsole.Write(usersTable);

            AnsiConsole.Write(new Rule("[yellow]Opcje[/]").LeftAligned());

            Dictionary<ConsoleKey, string> usersOptions = new()
            {
                { ConsoleKey.W, "Dodaj nowego domownika" },
                { ConsoleKey.D, "Modyfikuj istniejącego domownika" },
                { ConsoleKey.U, "Wróć do menu" }
            };

            ConsoleKey selector;

            var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($" \t\t\t\t [darkorange]Co chcesz zrobić?[/]")
                .PageSize(5)
                .MoreChoicesText("[grey](Przesuwaj w górę i w dół, a wybraną opcję zatwierdź klawiszem ENTER)[/]")
                .AddChoices(usersOptions.Values)
            );

            AnsiConsole.WriteLine($"Wybrałeś opcję: {selectedOption}");

            selector = usersOptions.FirstOrDefault(option => option.Value == selectedOption).Key;

            switch (selector)
            {
                case ConsoleKey.W:
                    AddNewUser(usersList);
                    break;

                case ConsoleKey.D:
                    Console.Write("Podaj id wybranego domownika: ");
                    string inputID = Console.ReadLine();
                    int selectedID = -1;
                    if (int.TryParse(inputID, out selectedID) && usersList.ContainsKey(selectedID))
                    {
                        EditExistingUser(selectedID, usersList);
                        return;
                    }
                    Console.WriteLine("Brak użytkownika zapisanego pod wybraną pozycją!");
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