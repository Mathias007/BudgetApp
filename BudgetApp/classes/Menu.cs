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
            { "[s]", "Dodaj nowe transakcje" },
            { "[a]", "Edytuj i usuwaj istniejące transakcje" },
            { "[d]", "Wyświetl transakcje" },
            { "[f]", "Wyświetl listę kategorii"},
            { "[c]", "Sprawdź stan konta"}
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

        public void ShowUsersList()
        {
            Console.WriteLine("Lista wszystkich domowników:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" + [0]: dodaj nowego domownika"); //
            printUserList(false);
            // Dodaj usera | komentaż daję żebyś ogarnął co dopisałem, usuń komentaż
            Console.WriteLine("Wybierz opcje/id, zostaw puste żeby pominąć[??] nie wiem jak to opisać żeby miało sens"); //help
            int consoleID = GetConsoleInput<User>.GetUserInputID(usersList, false);
            if (consoleID == -1)
            {
                Console.Clear();
                return;
            }
            if (consoleID == 0)
            {
                int newUserID = usersList.Count == 0 ? 1 : usersList.Keys.Max() + 1;
                Console.WriteLine("Imię: ");
                string firstName = Console.ReadLine();
                Console.WriteLine("Nazwisko: ");
                string lastName = Console.ReadLine();
                User addingUser = new User(newUserID, firstName, lastName);
                usersList.Add(addingUser.UserID, addingUser);
                return;
            }
            Console.WriteLine($"Wpisz nowe imię, zostaw puste żeby pominiąć({usersList[consoleID].UserFirstName}): ");
            string newFirstName = Console.ReadLine();
            usersList[consoleID].UserFirstName = String.IsNullOrWhiteSpace(newFirstName) ? usersList[consoleID].UserFirstName : newFirstName;

            Console.WriteLine($"Wpisz nowe nazwisko, zostaw puste żeby pominiąć({usersList[consoleID].UserLastName}): ");
            string newLastName = Console.ReadLine();
            usersList[consoleID].UserLastName = String.IsNullOrWhiteSpace(newLastName) ? usersList[consoleID].UserLastName : newLastName;

            Console.WriteLine($"Domownik jest aktywny({usersList[consoleID].UserIsActive})? (t/n), zostaw puste żeby nie zmieniać");
            string newActiveStatus = Console.ReadLine().ToUpper();
            if (newActiveStatus.Equals("T"))
                usersList[consoleID].UserIsActive = true;
            else if (newActiveStatus.Equals("N"))
                usersList[consoleID].UserIsActive = false;
            // koniec dodaj usera
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void ShowCategoriesList()
        {
            Console.WriteLine("Lista wszystkich kategorii:");
            Console.ForegroundColor = ConsoleColor.Yellow;


            Console.WriteLine(" + [0]: dodaj kategorię");
            printCategoriesList(false);
            //dodaj kategorię
            Console.WriteLine("Wybierz opcje/id, zostaw puste żeby pominąć[??] nie wiem jak to opisać żeby miało sens"); //help
            int consoleID = GetConsoleInput<Category>.GetUserInputID(categoriesList, false);
            if (consoleID == -1)
            {
                Console.Clear();
                return;
            }
            if (consoleID == 0)
            {
                int newCategoryID = categoriesList.Count == 0 ? 1 : categoriesList.Keys.Max() + 1;
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
                Category addingCategory = new Category(newCategoryID, incomeOrExpense, categoryName);
                categoriesList.Add(addingCategory.CategoryID, addingCategory);
                return;
            }
            Console.WriteLine($"Wpisz nową nazwę kategorii, zostaw puste żeby pominąć({categoriesList[consoleID].CategoryName}): ");
            string newCategoryName = Console.ReadLine();
            categoriesList[consoleID].CategoryName = String.IsNullOrWhiteSpace(newCategoryName) ? categoriesList[consoleID].CategoryName : newCategoryName;

            Console.WriteLine($"Kategoria jest aktywna({categoriesList[consoleID].IsActive})? (t/n), zostaw puste żeby nie zmieniać ");
            string newActiveStatus = Console.ReadLine().ToUpper();
            if (newActiveStatus.Equals("T"))
                categoriesList[consoleID].IsActive = true;
            else if (newActiveStatus.Equals("N"))
                categoriesList[consoleID].IsActive = false;
            // koniec dodawania kategorii
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public void AddTransactionReworked()
        {
            int transactionID = transactionsList.Count == 0 ? 1 : transactionsList.Keys.Max() + 1;

            Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer: ");
            printCategoriesList(true);
            int selectedCategoryID = GetConsoleInput<Category>.GetUserInputID(categoriesList, true);

            double transactionAmmount = UserInputTransactionAmmount(false);

            Console.Write("Wprowadź opis transakcji (pole opcjonalne): ");
            string description = Console.ReadLine();

            Console.WriteLine("Do którego domownika należy ta transakcja?");
            int selectedUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);

            DateTimeOffset date = ChooseDateOfTransaction();

            transactionsList.Add(transactionID, new Transaction(transactionID, categoriesList[selectedCategoryID], transactionAmmount, description, usersList[selectedUserID], date));
        }
        public void EditTransactionReworked()
        {
            Console.Write("Czy chcesz wyświetlić listę transakcji przed edycją? [T/N]");
            if (Console.ReadLine().ToUpper() == "T") PrintTransactionList();

            Console.Write("Wpisz ID poszukiwanej transakcji: ");
            int selectedTransactionID = int.Parse(Console.ReadLine()); //TODO napisać metodę do walidacji inputu

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

                    Console.WriteLine($"Wpisz nową kwotę ({oldTransaction.TransactionAmount}), zostaw puste żeby nie zmieniać");
                    double newAmmount = UserInputTransactionAmmount(true);
                    transactionsList[selectedTransactionID].TransactionAmount = newAmmount == -1 ? transactionsList[selectedTransactionID].TransactionAmount : newAmmount;

                    Console.WriteLine($"Wpisz nowy opis transakcji {oldTransaction.TransactionDescription}, zostaw puste żeby nie zmieniać"); //ogarnąć żeby wyświetlało to estetycznie
                    string newDescription = Console.ReadLine();
                    transactionsList[selectedTransactionID].TransactionDescription = string.IsNullOrWhiteSpace(newDescription) ? transactionsList[selectedTransactionID].TransactionDescription : newDescription;

                    Console.WriteLine($"Przypisz tą transakcje do innego domownika ({oldTransaction.TransactionUser.UserFirstName} {oldTransaction.TransactionUser.UserLastName}), zostaw puste żeby nie zmieniać");
                    printUserList(true);
                    int selectedNewUserID = GetConsoleInput<User>.GetUserInputID(usersList, true);
                    transactionsList[selectedTransactionID].TransactionUser = selectedNewUserID == -1 ? transactionsList[selectedTransactionID].TransactionUser : usersList[selectedNewUserID];

                    Console.WriteLine($"Zmienić datę tej transakcji? {oldTransaction.TransactionDate.ToString("dd-MM-yyyy")} (t/n)");
                    if (Console.ReadLine().ToUpper().Equals("T"))
                    {
                        transactionsList[selectedTransactionID].TransactionDate = ChooseDateOfTransaction();
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
            //do implementacji
        }

        private void PrintMenuHeader(User user)
        {
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
                Budget.SaveTransactionList(_budget._budget, Budget.fileNames["Transactions"]);
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
                    PrintMenuHeader(user);

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    Console.Clear();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.W:
                            ShowUsersList();
                            break;

                        case ConsoleKey.S:
                            AddTransactionReworked();
                            break;

                        case ConsoleKey.A:
                            EditTransactionReworked();
                            break;

                        case ConsoleKey.D:
                            PrintTransactionList();
                            break;

                        case ConsoleKey.F:
                            ShowCategoriesList();
                            break;

                        case ConsoleKey.C:
                            _budget.CalculateBalance();
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
        public DateTimeOffset ChooseDateOfTransaction()
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
                if (DateTimeOffset.TryParseExact(consoleInput.Trim(),"dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out returnDate))
                {
                    return returnDate;
                }
                Console.WriteLine($"Nieprawidłowy format daty! ma być w formacie DD-MM-RRRR, przykład - dzisiaj jest {DateTimeOffset.Now.ToString("dd-MM-yyyy")}");
            }
        }
        private double UserInputTransactionAmmount(bool allowEmpty)
        {
            Console.Write("Wprowadź kwotę PLN");
            double transactionAmmount = -1;
            string consoleInput = Console.ReadLine();
            while (true)
            {
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
                Console.WriteLine("Wpisz id które chcesz wybrać:");
                string selectedID = Console.ReadLine();
                int returnID = -1;
                while (true)
                {
                    if (string.IsNullOrWhiteSpace(selectedID))
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
                    selectedID = Console.ReadLine();
                }
            }
        }
    }
}
