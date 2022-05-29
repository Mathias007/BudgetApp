using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BudgetApp
{
    abstract class BudgetService : IBudgetService
    {
        public string GetDatabasePath()
        {
            string fileName = "TransactionsList.json";
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\{fileName}"));
        }
        public void CreateBudgetDB()
        {
            using FileStream createStream = File.Create(GetDatabasePath());
        }
        public async Task ShowTransactionList() 
        {
            using FileStream openStream = File.OpenRead(GetDatabasePath());
            Transaction[]? transactionsList =
                await JsonSerializer.DeserializeAsync<Transaction[]>(openStream);

            //Console.WriteLine($"ID: {transaction?.TransactionID}");
            //Console.WriteLine($"Category: {transaction?.TransactionCategory}");
            //Console.WriteLine($"Date: {transaction?.TransactionDate}");               
        }
        public double CalculateBalanceValue() => throw new NotImplementedException();
        public double CalculateBudgetStructure() => throw new NotImplementedException();
    }
}
