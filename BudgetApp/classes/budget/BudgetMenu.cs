using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityLibraries;

namespace BudgetApp
{
    public class BudgetMenu : Budget, IBudgetMenu
    {
        private static bool _isProgramOpen = true;
        private static readonly Dictionary<ConsoleKey, string> _programOptions = new()
        {
            { ConsoleKey.W, "Domownicy" },
            { ConsoleKey.D, "Transakcje" },
            { ConsoleKey.F, "Kategorie"},
            { ConsoleKey.C, "Podsumowanie"},
            { ConsoleKey.U, "Wyjście"}
        };
        private static ConsoleKey _selector;

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<ConsoleKey, string> ProgramOptions { get => _programOptions; }
        public ConsoleKey OptionSelector { get => _selector; set => _selector = value; }

        private static void PrintMenuHeader(User user)
        {
            Console.Clear();

            var font = FigletFont.Load(UtilitiesLibrary.GetDatabasePath("assets/starwars.flf"));

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
                    .Title($" \t\t\t\t [darkorange]Witamy [u]{user.UserFirstName} {user.UserLastName}[/] w aplikacji budżetowej![/] \n \t\t\t\t [green]Aby przejść dalej, wybierz opcję z listy poniżej:[/]")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Przesuwaj w górę i w dół, a wybraną opcję zatwierdź klawiszem ENTER)[/]")
                    .AddChoices(_programOptions.Values)                    
                    );

            AnsiConsole.WriteLine($"Wybrałeś opcję: {selectedOption}");

            _selector = _programOptions.FirstOrDefault(option => option.Value == selectedOption).Key;
        }

        public static void ExitFromProgram()
        {
            if (AnsiConsole.Confirm("Czy chcesz wyjść z programu?"))
            {
                AnsiConsole.MarkupLine("Dziękujemy za skorzystanie z aplikacji budżetowej");
                SaveTransactionList(transactionsList, fileNames["Transactions"]);
                SaveCategoryList(categoriesList, fileNames["Categories"]);
                SaveUserList(usersList, fileNames["Users"]);
                _isProgramOpen = !_isProgramOpen;
            }
        }

        public void HandleMenu(User user)
        {
            if (user.UserIsActive)
            {
                do
                {
                    PrintMenuHeader(user);

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
                            ManageBudgetSummary();
                            break;

                        case ConsoleKey.U:
                            ExitFromProgram();
                            break;

                        default:
                            ExitFromProgram();
                            break;
                    }
                } while (_isProgramOpen);
            } else
            {
                Console.WriteLine("Konto nieaktywne - brak uprawnień");
                ExitFromProgram();
            }
                Console.ReadKey();
        }      
    }
}
