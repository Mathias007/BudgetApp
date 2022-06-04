using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BudgetApp
{
    class Transaction : BudgetService, ITransaction
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

        public Transaction CreateNewTransaction(int id, Category category, double amount, string description, User user, DateTimeOffset date)
        {
            var testTransaction = new Transaction
            (
                 0,
                 new Category("income", "transaction"),
                 2137.5,
                 "Testowa transakcja",
                 new User("Jan", "Kowalski", true, true),
                 DateTime.Parse("2019-08-01")
            );

            // method to implement - look at AddTransactions in Menu class (separate logic)

            return testTransaction;

        }
        public Transaction FindTransactionByID(int id)
        {
            var testTransaction = new Transaction
            (
                 0,
                 new Category("income", "transaction"),
                 2137.5,
                 "Testowa transakcja",
                 new User("Jan", "Kowalski", true, true),
                 DateTime.Parse("2019-08-01")
            );

            // method to implement look at EditTransactions in Menu class (separate logic)

            return testTransaction;
        }
        public Transaction ModifySelectedTransaction(int id, Category category, double amount, string description, User user, DateTimeOffset date)
        {
            Transaction modyfyingTransaction = FindTransactionByID(id);

            // method to implement look at EditTransactions in Menu class (separate logic)

            return modyfyingTransaction;
        }
        public Transaction RemoveSelectedTransaction(int id)
        {
            Transaction removingTransaction = FindTransactionByID(id);

            // method to implement look at EditTransactions in Menu class (separate logic)

            return removingTransaction;
        }
    }
}
