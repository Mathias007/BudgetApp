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
