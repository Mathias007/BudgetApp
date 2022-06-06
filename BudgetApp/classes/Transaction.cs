using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public class Transaction : BudgetService, ITransaction
    {
        private int _id;
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

        public static Transaction CreateNewTransaction(int transactionID, Dictionary<int, Category> categoriesList, User user)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Wybierz kategorię transakcji z listy poniżej, wpisując jej numer: ");
            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                Console.WriteLine($" + {record.Key}: {record.Value.CategoryName}");
            }
            int selectedCategoryID = int.Parse(Console.ReadLine());

            Console.Write("Wprowadź kwotę PLN (wartość bezwzględna - w przypadku wyboru kategorii wydatku liczba zostanie potraktowana jako ujemna): ");
            double amount = double.Parse(Console.ReadLine());

            Console.Write("Wprowadź opis transakcji (pole opcjonalne): ");
            string description = Console.ReadLine();

            DateTimeOffset date = DateTimeOffset.Now;

            return new Transaction(
                transactionID,
                categoriesList[selectedCategoryID],
                amount,
                description,
                user,
                date
            );
        }

        public static Transaction FindTransactionByID(Dictionary<int, Transaction> transactionsList)
        {
            Console.Write("Wpisz ID poszukiwanej transakcji: ");
            int selectedTransactionID = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (transactionsList.ContainsKey(selectedTransactionID))
            {
                Console.WriteLine("Pod wpisanym kluczem znaleziono poniższą transakcję:");
                transactionsList[selectedTransactionID].PrintProperties();
                return transactionsList[selectedTransactionID];
            }
            else
            {
                Console.WriteLine("Nie znaleziono transakcji o takim ID.");
                return null;
            }
        }

        public static Transaction ModifySelectedTransaction(Transaction modyfingTransaction, Dictionary<int, Category> categoriesList)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Wybierz nową kategorię transakcji z listy poniżej, wpisując jej numer ");
            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                Console.WriteLine($" + {record.Key}: {record.Value.CategoryName}");
            }
            int selectedCategoryID = int.Parse(Console.ReadLine());

            Console.Write("Wprowadź nową kwotę w PLN: ");
            double amount = double.Parse(Console.ReadLine());

            Console.Write("Wprowadź nowy opis transakcji: ");
            string description = Console.ReadLine();

            return new Transaction(
                modyfingTransaction.TransactionID,
                categoriesList[selectedCategoryID],
                amount,
                description,
                modyfingTransaction.TransactionUser,
                modyfingTransaction.TransactionDate
            );
        }

        public static Dictionary<int, Transaction> RemoveSelectedTransaction(int id, Dictionary<int, Transaction> transactionsList)
        {
            // Transaction removingTransaction = transactionsList[id];
            Console.WriteLine($"Wybrałeś usuwanie transakcji zapisanej pod numerem {id}");
            Console.Write("Czy potwierdzasz usuwanie [T/N]? Operacja jest nieodwracalna: ");

            string finalDecisionKey = Console.ReadLine();

            if (finalDecisionKey.ToUpper() == "T") transactionsList.Remove(id);

            Console.WriteLine("Operacja usuwania transakcji zakończyła się powodzeniem");

            return transactionsList;
        }

        public void PrintProperties()
        {
            Console.WriteLine($"id: {_id} \n" +
                $"categoryName: {_category.CategoryName} \n" +
                $"categoryType: {_category.CategoryType} \n" +
                $"amount: {_amount} \n" +
                $"description: {_description} \n" +
                $"user: {_user.UserFirstName} {_user.UserLastName} \n" +
                $"date: {_date}");
        }
    }
}
