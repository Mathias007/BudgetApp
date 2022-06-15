using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Menu : Budget, IMenu
    {
        private static bool _isProgramOpen = true;
        private static readonly Dictionary<ConsoleKey, string> _programOptions = new()
        {
            { ConsoleKey.W, "Domownicy" },
            { ConsoleKey.D, "Transakcje" },
            { ConsoleKey.F, "Kategorie"},
            { ConsoleKey.C, "Wyświetl transakcje wg kategorii"},
            { ConsoleKey.U, "Wyświetl transakcje wg użytkownika" }
        };
        private static ConsoleKey _selector;

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<ConsoleKey, string> ProgramOptions { get => _programOptions; }

        private static void PrintMenuHeader(User user)
        {
            Console.Clear();
            // Console.WriteLine($"Witamy {user.UserFirstName} {user.UserLastName} w aplikacji budżetowej. Aby przejść dalej, wybierz opcję z listy poniżej:");

            //foreach (KeyValuePair<string, string> option in _programOptions)
            //{
            //    Console.WriteLine($" {option.Key} - {option.Value}");
            //}

            var font = FigletFont.Load(GetDatabasePath("assets/starwars.flf"));

            AnsiConsole.Write(
                new FigletText(font, "Budget")
                    .Centered()
                    .Color(Color.Red));
            AnsiConsole.Write(
                new FigletText(font, "App")
                    .Centered()
                    .Color(Color.Blue));

            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($" [darkorange]Witamy [u]{user.UserFirstName} {user.UserLastName}[/] w aplikacji budżetowej![/] \n [green]Aby przejść dalej, wybierz opcję z listy poniżej:[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Przesuwaj w górę i w dół, a wybraną opcję zatwierdź klawiszem ENTER)[/]")
                    .AddChoices(_programOptions.Values)
                    );

            AnsiConsole.WriteLine($"Wybrałeś opcję: {selectedOption}");

            _selector = _programOptions.FirstOrDefault(option => option.Value == selectedOption).Key;
        }
        public static void ManageProgramWorking()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Czy chcesz wyjść z programu? \n Naciśnij [t] - tak, [n] - nie");

            if (Console.ReadKey().Key == ConsoleKey.T)
            {
                Console.WriteLine("\n Dziękujemy za skorzystanie z aplikacji budżetowej");
                SaveTransactionList(transactionsList, fileNames["Transactions"]);
                SaveCategoryList(categoriesList, fileNames["Categories"]);
                SaveUserList(usersList, fileNames["Users"]);
                _isProgramOpen = !_isProgramOpen;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void HandleMenu(User user)
        {
            if (user.UserIsActive)
            {
                do
                {
                    PrintMenuHeader(user);

                    // ConsoleKeyInfo keyInfo = Console.ReadKey();

                    Console.Clear();

                    switch (_selector)
                    {
                        case ConsoleKey.W:
                            User.ManageUsers(usersList);
                            break;

                        case ConsoleKey.D:
                            Transaction.ManageTransactions(transactionsList, categoriesList, usersList);
                            break;

                        case ConsoleKey.F:
                            Category.ManageCategories(categoriesList);
                            break;

                        case ConsoleKey.C:
                            User.PrintUsers(false, usersList);
                            int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, false);
                            if (selectedUserID == -1)
                                return;
                            Transaction.GetTransactionByUser(selectedUserID, transactionsList, categoriesList, usersList );
                            break;

                        case ConsoleKey.U:
                            Category.PrintCategories(false, categoriesList);
                            int selectedConsoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
                            if (selectedConsoleID == -1)
                                return;
                            Transaction.GetTransactionByCategory(selectedConsoleID, transactionsList, categoriesList, usersList);
                            break;

                        default:
                            ManageProgramWorking();
                            break;
                    }
                } while (_isProgramOpen);
            } else
            {
                Console.WriteLine("Konto nieaktywne - brak uprawnień");
                ManageProgramWorking();
            }
                Console.ReadKey();
        }      
    }
}
