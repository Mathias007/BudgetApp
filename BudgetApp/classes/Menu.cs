using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class Menu : IMenu
    {
        private bool _isProgramOpen = true;
        private readonly Dictionary<string, string> _programOptions = new()
        {
            { "[w]", "Wyświetl listę domowników" },
            { "[s]", "Dodaj nowe transakcje" },
            { "[a]", "Edytuj i usuwaj istniejące transakcje" },
            { "[d]", "Wyświetl transakcje" }
        };

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<string, string> ProgramOptions { get => _programOptions; }

        public void ShowUsersList(Dictionary<int, User> usersList)
        {
            Console.WriteLine("Lista wszystkich domowników:");
            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (KeyValuePair<int, User> record in usersList)
            {
                Console.WriteLine($" + {record.Key}: {record.Value}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void AddTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            int transactionID = transactionsList.Keys.Max() + 1;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer ");
            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                Console.WriteLine($" + {record.Key}: {record.Value}");
            }
            int selectedCategoryID = int.Parse(Console.ReadLine());

            Console.WriteLine("Wprowadź kwotę PLN (wartość bezwzględna - w przypadku wyboru kategorii wydatku liczba zostanie potraktowana jako ujemna): ");
            double amount = double.Parse(Console.ReadLine());

            Console.WriteLine("Wprowadź opis transakcji (pole opcjonalne): ");
            string description = Console.ReadLine();

            DateTimeOffset date = DateTimeOffset.Now;

            Transaction addingTransaction = new(
                transactionID,
                categoriesList[selectedCategoryID],
                amount,
                description,
                user,
                date
            );

            transactionsList.Add(transactionID, addingTransaction);
        }

        public void EditTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            Console.WriteLine("Czy chcesz wyświetlić listę transakcji przed edycją? [T/N]");
            string showingTransactionsList = Console.ReadLine();

            if (showingTransactionsList.ToUpper() == "T") ShowTransactions(transactionsList, categoriesList, user);

            Console.WriteLine("Wpisz ID poszukiwanej transakcji: ");
            int selectedTransactionID = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (transactionsList.ContainsKey(selectedTransactionID))
            {
                Console.WriteLine("Co zamierzasz zrobić z wybraną transakcją? [e] - edycja, [d] - usuwanie");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.E:
                        // EDYCJA DANYCH TRANSAKCJI (można uwzględnić uprawnienia - pole isAdmin)
                        break;
                    case ConsoleKey.D:
                        // USUWANIE DANYCH TRANSAKCJI (można uwzględnić uprawnienia - pole isAdmin)
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór");
                        ManageProgramWorking();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono takiej transakcji");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void ShowTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            static void ShowFilterOptions(bool isAdmin) { 

                string adminWelcome = "Uprawnienia administracyjne - dostęp do pełnej listy transakcji";
                string nonAdminWelcome = "Brak uprawnień administracyjnych - dostęp do listy własnych transakcji";

                Console.WriteLine($" {(isAdmin ? adminWelcome : nonAdminWelcome)} ");
                Dictionary<string, string> filterOptions = new()
                {
                    { "[a]", "Wyświetl wszystkie (dostępne) transakcje" },
                    { "[m]", "Wyświetl transakcje dla wybranego miesiąca" },
                    { "[u]", "Wyświetl transakcje dla wybranego użytkownika" },
                    { "[c]", "Wyświetl transakcje według kategorii" },
                    { "[p]", "Wyświetl kwoty w przedziale" },
                };

                foreach (KeyValuePair<string, string> option in filterOptions)
                {
                    Console.WriteLine($" {option.Key} - {option.Value}");
                }
            }

            if (user.UserIsAdmin)
            {
                    ShowFilterOptions(user.UserIsAdmin);
            } 
            else
            {
                    ShowFilterOptions(user.UserIsAdmin);
            }
        }
        private void PrintMenuHeader()
        {
            Console.WriteLine("Witamy w aplikacji budżetowej. Aby przejść dalej, wybierz opcję z listy poniżej:");

            foreach (KeyValuePair<string, string> option in _programOptions)
            {
                Console.WriteLine($" {option.Key} - {option.Value}");
            }
        }

        public void ManageProgramWorking()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Czy chcesz wyjść z programu? \n Naciśnij [t] - tak, [n] - nie");

            if (Console.ReadKey().Key == ConsoleKey.T)
            {
                Console.WriteLine("\n Dziękujemy za skorzystanie z aplikacji budżetowej");
                _isProgramOpen = !_isProgramOpen;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void HandleMenu(Dictionary<int, User> usersList, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            if (user.UserIsActive)
            {
                do
                {
                    PrintMenuHeader();

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    Console.Clear();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.W:
                            ShowUsersList(usersList);
                            break;

                        case ConsoleKey.S:
                            AddTransactions(transactionsList, categoriesList, user);
                            break;

                        case ConsoleKey.A:
                            EditTransactions(transactionsList, categoriesList, user);
                            break;

                        case ConsoleKey.D:
                            ShowTransactions(transactionsList, categoriesList, user);
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
