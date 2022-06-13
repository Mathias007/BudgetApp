using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.classes
{

    // interface:
    //  static Transaction CreateNewTransaction(int transactionID, Dictionary<int, Category> categoriesList, User user) => throw new NotImplementedException($"{transactionID}, {categoriesList}, {user}");
    //  static Transaction FindTransactionByID(Dictionary<int, Transaction> transactionsList) => throw new NotImplementedException($"{transactionsList[0]}");
    // static Transaction ModifySelectedTransaction(Transaction modyfingTransaction, Dictionary<int, Category> categoriesList) => throw new NotImplementedException($"{modyfingTransaction.TransactionID}, {categoriesList[0]}");
    // static Dictionary<int, Transaction> RemoveSelectedTransaction(int id, Dictionary<int, Transaction> transactionsList) => throw new NotImplementedException($"{transactionsList[id]}");

    class OldTransactionService
    {
        
        // metody aktualnie niewykorzystywane - do wyrzucenia przed prezentacją

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


        //////public static Transaction CreateNewTransaction(int transactionID, Dictionary<int, Category> categoriesList, User user)
        //////{
        //////    Console.ForegroundColor = ConsoleColor.Green;
        //////    Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer: ");
        //////    foreach (KeyValuePair<int, Category> record in categoriesList)
        //////    {
        //////        if (record.Value.CategoryIsActive)
        //////        {
        //////            Console.WriteLine($" + {record.Key}: {record.Value.CategoryName}");
        //////        }
        //////    }
        //////    int selectedCategoryID = int.Parse(Console.ReadLine());
        //////    while (true)
        //////    {
        //////        if (!categoriesList.ContainsKey(selectedCategoryID) || !categoriesList[selectedCategoryID].CategoryIsActive)
        //////        {
        //////            Console.WriteLine("nieprawidłowe id, spróbuj jeszcze raz z innym id");
        //////            selectedCategoryID = int.Parse(Console.ReadLine());
        //////        }
        //////        break;
        //////    }
        //////    Console.Write("Wprowadź kwotę PLN (wartość bezwzględna - w przypadku wyboru kategorii wydatku liczba zostanie potraktowana jako ujemna): ");
        //////    double amount = double.Parse(Console.ReadLine());

        //////    Console.Write("Wprowadź opis transakcji (pole opcjonalne): ");
        //////    string description = Console.ReadLine();

        //////    DateTimeOffset date = DateTimeOffset.Now;

        //////    return new Transaction(
        //////        transactionID,
        //////        categoriesList[selectedCategoryID],
        //////        amount,
        //////        description,
        //////        user,
        //////        date
        //////    );
        //////}

        //////public static Transaction FindTransactionByID(Dictionary<int, Transaction> transactionsList)
        //////{
        //////    Console.Write("Wpisz ID poszukiwanej transakcji: ");
        //////    int selectedTransactionID = int.Parse(Console.ReadLine());

        //////    Console.ForegroundColor = ConsoleColor.Cyan;
        //////    if (transactionsList.ContainsKey(selectedTransactionID))
        //////    {
        //////        Console.WriteLine("Pod wpisanym kluczem znaleziono poniższą transakcję:");
        //////        transactionsList[selectedTransactionID].PrintProperties();
        //////        return transactionsList[selectedTransactionID];
        //////    }
        //////    else
        //////    {
        //////        Console.WriteLine("Nie znaleziono transakcji o takim ID.");
        //////        return null;
        //////    }
        //////}

        //////public static Transaction ModifySelectedTransaction(Transaction modyfingTransaction, Dictionary<int, Category> categoriesList)
        //////{
        //////    Console.ForegroundColor = ConsoleColor.Green;
        //////    Console.WriteLine("Wybierz nową kategorię transakcji z listy poniżej, wpisując jej numer ");
        //////    foreach (KeyValuePair<int, Category> record in categoriesList)
        //////    {
        //////        if (record.Value.CategoryIsActive)
        //////        {
        //////            Console.WriteLine($" + {record.Key}: {record.Value.CategoryName}");
        //////        }
        //////    }
        //////    int selectedCategoryID = int.Parse(Console.ReadLine());
        //////    while (true)
        //////    {
        //////        if (!categoriesList.ContainsKey(selectedCategoryID) || !categoriesList[selectedCategoryID].CategoryIsActive)
        //////        {
        //////            Console.WriteLine("nieprawidłowe id, spróbuj jeszcze raz z innym id");
        //////            selectedCategoryID = int.Parse(Console.ReadLine());
        //////        }
        //////        break;
        //////    }
        //////    Console.Write("Wprowadź nową kwotę w PLN: ");
        //////    double amount = double.Parse(Console.ReadLine());

        //////    Console.Write("Wprowadź nowy opis transakcji: ");
        //////    string description = Console.ReadLine();

        //////    return new Transaction(
        //////        modyfingTransaction.TransactionID,
        //////        categoriesList[selectedCategoryID],
        //////        amount,
        //////        description,
        //////        modyfingTransaction.TransactionUser,
        //////        modyfingTransaction.TransactionDate
        //////    );
        //////}

        ////// public static Dictionary<int, Transaction> RemoveSelectedTransaction(int id, Dictionary<int, Transaction> transactionsList)
        //////{
        //////    // Transaction removingTransaction = transactionsList[id];
        //////    Console.WriteLine($"Wybrałeś usuwanie transakcji zapisanej pod numerem {id}");
        //////    Console.Write("Czy potwierdzasz usuwanie [T/N]? Operacja jest nieodwracalna: ");

        //////    string finalDecisionKey = Console.ReadLine();

        //////    if (finalDecisionKey.ToUpper() == "T") transactionsList.Remove(id);

        //////    Console.WriteLine("Operacja usuwania transakcji zakończyła się powodzeniem");

        //////    return transactionsList;
        //////}



    }
}
