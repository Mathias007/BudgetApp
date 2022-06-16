using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface ITransaction
    {
        int TransactionID { get; set; }
        Category TransactionCategory { get; set; }
        double TransactionAmount { get; set; }
        string TransactionDescription { get; set; }
        User TransactionUser { get; set; }
        DateTimeOffset TransactionDate { get; set; }

        static Dictionary<int, Transaction> GetTransactionByCategory(int selectedCategoryID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList) => throw new NotImplementedException();
        static Dictionary<int, Transaction> GetTransactionByUser(int selectedUserID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList) => throw new NotImplementedException();
        static void AddNewTransaction(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList) => throw new NotImplementedException();
        static void EditExistingTransaction(int selectedTransactionID, Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList) => throw new NotImplementedException();
        static void ManageTransactions(Dictionary<int, Transaction> transactionsList, Dictionary<int, Category> categoriesList, Dictionary<int, User> usersList) => throw new NotImplementedException();

        public void PrintProperties();   
    }
}
