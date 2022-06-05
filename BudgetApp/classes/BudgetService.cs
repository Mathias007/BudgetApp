using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace BudgetApp
{  
    public abstract class BudgetService : IBudgetService
    {
        private static string GetDatabasePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\{fileName}"));
        }

        public static Dictionary<int, Transaction> LoadTransactionList(string fileName)
        {
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                Console.WriteLine("nie ma pliku do LoadTransactionList()");
                return null;
            }
            return JsonConvert.DeserializeObject<Dictionary<int, Transaction>>(File.ReadAllText(GetDatabasePath(fileName)));
        }

        public static void SaveTransactionList(Dictionary<int, Transaction> transactionsList, string fileName)
        {
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(fileName));
            }
            File.WriteAllText(GetDatabasePath(fileName), JsonConvert.SerializeObject(transactionsList));
        }

        public static List<User> LoadUserList(string fileName)
        {
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                Console.WriteLine("nie ma pliku do LoadUserList()");
                return null;
            }
            return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(GetDatabasePath(fileName)));
        }

        public static void SaveUserList(List<User> usersList, string fileName)
        {
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(fileName));
            }
            File.WriteAllText(GetDatabasePath(fileName), JsonConvert.SerializeObject(usersList));
        }

        public Dictionary<int, Transaction> LoadDataFromDB() => throw new NotImplementedException();
        public Dictionary<int, Transaction> SaveDataToDB() => throw new NotImplementedException();
        public double CalculateBalanceValue() => throw new NotImplementedException();
        public double CalculateBudgetStructure() => throw new NotImplementedException();
    }
}
