using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Menu : Budget, IMenu
    {
       // private static readonly Dictionary<int, Transaction> transactionsList;
       // private static readonly Dictionary<int, User> usersList;
       // private static readonly Dictionary<int, Category> categoriesList;
        private static bool _isProgramOpen = true;
        private static readonly Dictionary<string, string> _programOptions = new()
        {
            { "[w]", "Wyświetl listę domowników" },
            { "[d]", "Wyświetl transakcje" },
            { "[f]", "Wyświetl listę kategorii"},
            { "[c]", "Wyświetl transakcje wg kategorii"},
            { "[u]", "Wyświetl transakcje wg użytkownika" }
        };
        // private Budget _budget;

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<string, string> ProgramOptions { get => _programOptions; }

        //public Menu()
        //{
        //    transactionsList = LoadTransactionList(fileNames["Transactions"]);
        //    usersList = LoadUserList(fileNames["Users"]);
        //    categoriesList = LoadCategoryList(fileNames["Categories"]);
        //}

        private static void PrintMenuHeader(User user)
        {
            Console.Clear();
            Console.WriteLine($"Witamy {user.UserFirstName} {user.UserLastName} w aplikacji budżetowej. Aby przejść dalej, wybierz opcję z listy poniżej:");

            foreach (KeyValuePair<string, string> option in _programOptions)
            {
                Console.WriteLine($" {option.Key} - {option.Value}");
            }
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

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    Console.Clear();

                    switch (keyInfo.Key)
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
