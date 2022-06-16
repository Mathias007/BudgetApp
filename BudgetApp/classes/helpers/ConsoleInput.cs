using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public class GetConsoleInput<T> where T : ITransactionObject
    {
        internal static int GetUserInputID(Dictionary<int, T> transactionObjectDictionary, bool chooseOnlyActive)
        {
            Console.WriteLine("Wpisz id które chcesz wybrać:");
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
    public class GetConsoleInput
    {
        internal static DateTimeOffset ChooseDateOfTransaction()
        {
            Console.Write("Jeśli transakcja jest z dzisiaj, zostaw puste pole, w innym wypadku wprowadź datę w formacie DD-MM-RRRR: ");
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
        internal static double UserInputTransactionAmount(bool allowEmpty)
        {
            Console.WriteLine("Wprowadź kwotę PLN");
            double transactionAmmount = -1;
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (allowEmpty && string.IsNullOrWhiteSpace(consoleInput))
                {
                    return -1;
                }
                if (double.TryParse(consoleInput, out transactionAmmount))
                {
                    if (transactionAmmount >= 0)
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
