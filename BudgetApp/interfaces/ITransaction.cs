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

      //  static Transaction CreateNewTransaction(int transactionID, Dictionary<int, Category> categoriesList, User user) => throw new NotImplementedException($"{transactionID}, {categoriesList}, {user}");
      //  static Transaction FindTransactionByID(Dictionary<int, Transaction> transactionsList) => throw new NotImplementedException($"{transactionsList[0]}");
       // static Transaction ModifySelectedTransaction(Transaction modyfingTransaction, Dictionary<int, Category> categoriesList) => throw new NotImplementedException($"{modyfingTransaction.TransactionID}, {categoriesList[0]}");
       // static Dictionary<int, Transaction> RemoveSelectedTransaction(int id, Dictionary<int, Transaction> transactionsList) => throw new NotImplementedException($"{transactionsList[id]}");
    }
}
