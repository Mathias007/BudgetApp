using System;
using System.Threading.Tasks;

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

        Transaction CreateNewTransaction(int id, Category category, double amount, string description, User user, DateTimeOffset date);
        Transaction FindTransactionByID(int id);
        Transaction ModifySelectedTransaction(int id, Category category, double amount, string description, User user, DateTimeOffset date);
        Transaction RemoveSelectedTransaction(int id);
    }
}
