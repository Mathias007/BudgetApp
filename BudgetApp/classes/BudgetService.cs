using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BudgetApp
{  
    public abstract class BudgetService : IBudgetService
    {
        public static Dictionary<string, string> fileNames = new()
        {
            { "Transactions", "TransactionsList.json" },
            { "Users", "UserList.json" },
            { "Categories", "CategoriesList.json" },
        };

        private static string GetDatabasePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\{fileName}"));
        }

        public static Dictionary<int, Transaction> LoadTransactionList(string fileName)
        {
            //if (!File.Exists(GetDatabasePath(fileName)))
            //{
            //    using FileStream createStream = File.Create(GetDatabasePath(fileName));
            //    return new Dictionary<int, Transaction>();
            //}
            //return JsonConvert.DeserializeObject<Dictionary<int, Transaction>>(File.ReadAllText(GetDatabasePath(fileName)));
            return new Dictionary<int, Transaction>();
        }
        public static Dictionary<int, Category> LoadCategoryList(string fileName)
        {
            //if (!File.Exists(GetDatabasePath(fileName)))
            //{
            //    using FileStream createStream = File.Create(GetDatabasePath(fileName));
            //    return new Dictionary<int, Category>();
            //}
            //return JsonConvert.DeserializeObject<Dictionary<int, Category>>(File.ReadAllText(GetDatabasePath(fileName)));
            return new Dictionary<int, Category>();
        }
        public static Dictionary<int, User> LoadUserList(string fileName)
        {
            //if (!File.Exists(GetDatabasePath(fileName)))
            //{
            //    using FileStream createStream = File.Create(GetDatabasePath(fileName));
            //    return new Dictionary<int, User>();
            //}
            //return JsonConvert.DeserializeObject<Dictionary<int, User>>(File.ReadAllText(GetDatabasePath(fileName)));
            return new Dictionary<int, User>();
        }

        public static void SaveTransactionList(Dictionary<int, Transaction> transactionsList, string fileName)
        {
            File.WriteAllText(GetDatabasePath(fileName), JsonConvert.SerializeObject(transactionsList));
        }

        public static void SaveUserList(Dictionary<int, User> usersList, string fileName)
        {
            File.WriteAllText(GetDatabasePath(fileName), JsonConvert.SerializeObject(usersList));
        }
        public static void SaveCategoryList(Dictionary<int, Category> categoryList, string fileName)
        {
            File.WriteAllText(GetDatabasePath(fileName), JsonConvert.SerializeObject(categoryList));
        }
    }
}
