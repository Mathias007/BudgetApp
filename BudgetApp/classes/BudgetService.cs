using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BudgetApp
{
    public abstract class BudgetService : IBudgetService
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

        public Dictionary<int, Transaction> LoadDataFromDB() => throw new NotImplementedException();
        public Dictionary<int, Transaction> SaveDataToDB() => throw new NotImplementedException();
        public double CalculateBalanceValue() => throw new NotImplementedException();
        public double CalculateBudgetStructure() => throw new NotImplementedException();
    }
}
