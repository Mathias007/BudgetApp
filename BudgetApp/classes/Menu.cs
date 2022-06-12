using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Menu : IMenu
    {
        private readonly Dictionary<int, Transaction> transactionsList;
        private readonly Dictionary<int, User> usersList;
        private readonly Dictionary<int, Category> categoriesList;
        private bool _isProgramOpen = true;
        private readonly Dictionary<string, string> _programOptions = new()
        {
            { "[w]", "Wyświetl listę domowników" },
            { "[d]", "Wyświetl transakcje" },
            { "[f]", "Wyświetl listę kategorii"},
            { "[c]", "Wyświetl transakcje wg kategorii"},
            { "[u]", "Wyświetl transakcje wg użytkownika" }
        };
        private Budget _budget;

        public bool IsProgramOpen { get => _isProgramOpen; set => _isProgramOpen = value; }
        public Dictionary<string, string> ProgramOptions { get => _programOptions; }

        public Menu()
        {
            transactionsList = BudgetService.LoadTransactionList(BudgetService.fileNames["Transactions"]);
            usersList = BudgetService.LoadUserList(BudgetService.fileNames["Users"]);
            categoriesList = BudgetService.LoadCategoryList(BudgetService.fileNames["Categories"]);
        }
        public Dictionary<int, Transaction> GetTransactionByCategory(int selectedCategoryID)
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
            PrintTransactionList(selectedCategoryTransaciton);
            return selectedCategoryTransaciton;
        }
        public Dictionary<int, Transaction> GetTransactionByUser(int selectedUserID)
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
            PrintTransactionList(selectedUserTransaciton);
            return selectedUserTransaciton;
        }

        public void ShowUsersList()
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich domowników:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            printUserList(false);
            Console.WriteLine(" \n -> Wybierz 0, aby dodać nowego domownika. \n -> Jeżeli chcesz zmodyfikować dane istniejącego domownika, wypisz jego numer ID. \n -> Aby wrócić do głównego menu, naciśnij ENTER, pozostawiając pole puste.");

            int consoleID = GetConsoleInput<User>.GetUserInputID(usersList, false);
            if (consoleID == -1)
            {
                Console.Clear();
                return;
            }
            if (consoleID == 0)
            {
                var createdUser = User.createUser(usersList);
                usersList.Add(createdUser.UserID,createdUser);
            }
            else if (usersList.ContainsKey(consoleID))
            {
                var editedUser = User.editUser(usersList[consoleID]);
                usersList[consoleID] = editedUser;
            }
            else
            {
                Console.WriteLine("Podane id nie istnieje");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void ShowCategoriesList()
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich kategorii:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            printCategoriesList(false);
            Console.WriteLine("Wybierz opcje/id, zostaw puste żeby pominąć[??] nie wiem jak to opisać żeby miało sens"); //help

            int consoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
            if (consoleID == -1)
            {
                Console.Clear();
                return;
            }
            else if (consoleID == 0)
            {
                var createdCategory = Category.createCategory(categoriesList);
                categoriesList.Add(createdCategory.CategoryID, createdCategory);
            }
            else if (categoriesList.ContainsKey(consoleID))
            {
                var editedCategory = Category.editCategory(categoriesList[consoleID]);
                categoriesList[consoleID] = editedCategory;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void AddTransactionReworked()
        {
            Console.Clear();
            int transactionID = transactionsList.Count == 0 ? 1 : transactionsList.Keys.Max() + 1;

            Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer: ");
            printCategoriesList(true);
            int selectedCategoryID = GetConsoleInput<Category>.GetUserInputID(categoriesList, true);
            Console.Clear();
            double transactionAmmount = GetConsoleInput.UserInputTransactionAmmount(false);
            Console.Clear();
            Console.Write("Wprowadź opis transakcji (pole opcjonalne): ");
            string description = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Do którego domownika należy ta transakcja?");
            printUserList(false);
            int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);
            Console.Clear();
            DateTimeOffset date = GetConsoleInput.ChooseDateOfTransaction();

            transactionsList.Add(transactionID, new Transaction(transactionID, categoriesList[selectedCategoryID], transactionAmmount, description, usersList[selectedUserID], date));
            Console.Clear();
        }
        public void EditTransactionReworked(int selectedTransactionID)
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
                    printCategoriesList(true);
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
                    printUserList(true);
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
                    ManageProgramWorking();
                    break;
            }
        }
        public void PrintTransactionList()
        {
            PrintTransactionList(transactionsList);
        }
        public void PrintTransactionList(Dictionary<int, Transaction> customTransactionList)
        {
            Console.Clear();
            Console.WriteLine("[0] - dodaj nową transakcje");
            bool colorChanger = false;
            foreach (KeyValuePair<int, Transaction> transaction in customTransactionList)
            {
                if (colorChanger)
                {
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("income"))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("expense"))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                }
                else if (!colorChanger)
                {
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("income"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (transaction.Value.TransactionCategory.CategoryType.Equals("expense"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                Console.WriteLine($"[{transaction.Key}] : ");
                transaction.Value.PrintProperties();
                colorChanger = colorChanger ? false : true ;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wybierz opcje/id, zostaw puste żeby wrócić do menu[??] nie wiem jak to opisać żeby miało sens"); //help
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    Console.Clear();
                    return;
                }
                if (consoleInput.Equals("0"))
                {
                    AddTransactionReworked();
                    return;
                }
                int selectedID = -1;
                if (int.TryParse(consoleInput, out selectedID) && transactionsList.ContainsKey(selectedID))
                {
                    EditTransactionReworked(selectedID);
                    return;
                }
                Console.WriteLine("podanego id nie ma na liscie transakcji");
            }
        }

        public void ShowTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, User user)
        {
            // do implementacji (można uwzględnić uprawnienia - pole isAdmin)
            //   ShowFilterOptions(user.UserIsAdmin);

            // Krok 1. Wybór kryterium filtrowania (switch <-> klawisz wg filterOptions)
            // Krok 2. Wygenerowanie nowej listy transakcji, powstałej w rezultacie przefiltrowania głównej listy.
            // Krok 3. Wyświetlenie w konsoli tabeli zawierającej dane z przefiltrowanej listy transakcji.
            // Krok 4. Obsłużenie możliwości wyczyszczenia okna i zastosowania nowego kryterium filtrowania.
            // Krok 5. Obsłużenie możliwości powrotu do głównego menu lub sekcji modyfikacji transakcji.

         // Przykład filtrowania (do potencjalnego wykorzystania)
         //   Dictionary<string, int> dict = new Dictionary<string, int>() {
         //   {"A", 1}, {"B", 2}, {"C", 3}, {"D", 4}, {"E", 5}
         //   };

         //   Dictionary<string, int> filtered = dict.Where(x => x.Value % 2 == 0)
         //                       .ToDictionary(x => x.Key, x => x.Value);

         //   Console.WriteLine(String.Join(", ", filtered));
        }

        private void PrintMenuHeader(User user)
        {
            Console.Clear();
            Console.WriteLine($"Witamy {user.UserFirstName} {user.UserLastName} w aplikacji budżetowej. Aby przejść dalej, wybierz opcję z listy poniżej:");

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
                BudgetService.SaveTransactionList(transactionsList, BudgetService.fileNames["Transactions"]);
                BudgetService.SaveCategoryList(categoriesList, BudgetService.fileNames["Categories"]);
                BudgetService.SaveUserList(usersList, BudgetService.fileNames["Users"]);
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
                            ShowUsersList();
                            break;

                        case ConsoleKey.D:
                            PrintTransactionList();
                            break;

                        case ConsoleKey.F:
                            ShowCategoriesList();
                            break;
                        case ConsoleKey.C:
                            printUserList(false);
                            int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, false);
                            if (selectedUserID == -1)
                                return;
                            GetTransactionByUser(selectedUserID);
                            break;
                        case ConsoleKey.U:
                            printCategoriesList(false);
                            int selectedConsoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
                            if (selectedConsoleID == -1)
                                return;
                            GetTransactionByCategory(selectedConsoleID);
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
        private void printUserList(bool onlyActive)
        {
            foreach (KeyValuePair<int, User> record in usersList)
            {
                if (onlyActive ? record.Value.IsActive : true)
                {
                    Console.WriteLine(
                        $" + [{record.Key}]: " +
                        $"{record.Value.UserFirstName} {record.Value.UserLastName} " +
                        $"{(record.Value.UserIsActive ? "AKTYWNY" : "NIEAKTYWNY")} " +
                        $"{(record.Value.UserIsAdmin ? "ADMINISTRATOR" : "USER")} ");
                }
            }
        }
        private void printCategoriesList(bool onlyActive)
        {
            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                if (onlyActive ? record.Value.IsActive : true)
                {
                    if (record.Value.CategoryType == "expense") Console.ForegroundColor = ConsoleColor.Red;
                    else if (record.Value.CategoryType == "income") Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine(
                            $" + {record.Key}: " +
                            $"{record.Value.CategoryName} ({record.Value.CategoryType})");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
        private class GetConsoleInput<T> where T : ITransactionObject
        {
            internal static int GetUserInputID(Dictionary<int, T> transactionObjectDictionary, bool chooseOnlyActive)
            {
                int returnID = -1;
                while (true)
                {
                    string selectedID = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(selectedID) && !chooseOnlyActive)
                    {
                        return -1;
                    }
                    if (selectedID.Equals("0") && !chooseOnlyActive) //zwracanie 0 ma tylko sens w metodach które mają chooseOnlyActive = false
                    {
                        return 0;
                    }
                    if (int.TryParse(selectedID, out returnID))
                    {
                        if (transactionObjectDictionary.ContainsKey(returnID) && (chooseOnlyActive ? transactionObjectDictionary[returnID].IsActive : true))
                        {
                            return returnID;
                        }
                        Console.WriteLine($"na liście nie istnieje podane id: {selectedID}");
                    }
                    else
                    {
                        Console.WriteLine($"podana wartość {selectedID} jest niepoprawna, wpisz wartość numeryczną");
                    }
                }
            }
        }
        private class GetConsoleInput
        {
            internal static DateTimeOffset ChooseDateOfTransaction()
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
                    if (DateTimeOffset.TryParseExact(consoleInput.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out returnDate))
                    {
                        return returnDate;
                    }
                    Console.WriteLine($"Nieprawidłowy format daty! ma być w formacie DD-MM-RRRR, przykład - dzisiaj jest {DateTimeOffset.Now.ToString("dd-MM-yyyy")}");
                }
            }
            internal static double UserInputTransactionAmmount(bool allowEmpty)
            {
                Console.WriteLine("Wprowadź kwotę PLN");
                double transactionAmmount = -1;
                while (true)
                {
                    string consoleInput = Console.ReadLine();
                    if (allowEmpty ? string.IsNullOrWhiteSpace(consoleInput) : false)
                    {
                        return -1;
                    }
                    if (double.TryParse(consoleInput, out transactionAmmount))
                    {
                        if (transactionAmmount >= 0) //można dodawać transakcje o wartości 0 bo czemu nie
                        {
                            return transactionAmmount;
                        }
                        Console.WriteLine("transakcja nie może być ujemna, jeśli chcesz odjąć wybierz kategorię wydatek");
                    }
                    Console.WriteLine("w tym miejscu wpisujemy wyłącznie liczbę");
                }
            }
        }

    }
}
