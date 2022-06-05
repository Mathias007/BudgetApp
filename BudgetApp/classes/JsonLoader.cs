using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BudgetApp
{
    public static class JsonLoader
    {
        private static string _transFilename = "TransactionsList.json";
        private static string _userFilename = "UserList.json";

        private static string GetDatabasePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\{fileName}"));
        }

        public static Dictionary<int, Transaction> LoadTransactionList()
        {
            if (!File.Exists(GetDatabasePath(_transFilename)))
            {
                Console.WriteLine("nie ma pliku do LoadTransactionList()");
                return null;
            }
            return JsonConvert.DeserializeObject<Dictionary<int, Transaction>>(File.ReadAllText(GetDatabasePath(_transFilename)));
        }

        public static void SaveTransactionList(Dictionary<int, Transaction> dict)
        {
            if (!File.Exists(GetDatabasePath(_transFilename)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(_transFilename));
            }
            File.WriteAllText(GetDatabasePath(_transFilename), JsonConvert.SerializeObject(dict));
        }

        public static List<User> LoadUserList()
        {
            if (!File.Exists(GetDatabasePath(_userFilename)))
            {
                Console.WriteLine("nie ma pliku do LoadUserList()");
                return null;
            }
            return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(GetDatabasePath(_userFilename)));
        }

        public static void SaveUserList(List<User> list)
        {
            if (!File.Exists(GetDatabasePath(_userFilename)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(_userFilename));
            }
            File.WriteAllText(GetDatabasePath(_userFilename), JsonConvert.SerializeObject(list));
        }
    }
}
