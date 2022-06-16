using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BudgetApp
{  
    public abstract class BudgetService : IBudgetService
    {
        internal static Dictionary<string, string> fileNames = new()
        {
            { "Transactions", "TransactionsList.json" },
            { "Users", "UserList.json" },
            { "Categories", "CategoriesList.json" },
        };

        public Dictionary<string, string> FileNames { get => fileNames; }


        public static string GetDatabasePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\{fileName}"));
        }

        public static Dictionary<int, Transaction> LoadTransactionList(string fileName)
        {
            Dictionary<int, Transaction> returnDictionary = null;
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(fileName));
            }
            else
            {
                returnDictionary = JsonConvert.DeserializeObject<Dictionary<int, Transaction>>(File.ReadAllText(GetDatabasePath(fileName)));
            }
            if (returnDictionary == null)
            {
                returnDictionary = new Dictionary<int, Transaction>();
            }
            return returnDictionary;
        }
        public static Dictionary<int, Category> LoadCategoryList(string fileName)
        {
            Dictionary<int, Category> returnDictionary = null;
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(fileName));
            }
            else
            {
                returnDictionary =  JsonConvert.DeserializeObject<Dictionary<int, Category>>(File.ReadAllText(GetDatabasePath(fileName)));
            }
            if (returnDictionary == null)
            {
                returnDictionary = new Dictionary<int, Category>();
            }
            return returnDictionary;
        }
        public static Dictionary<int, User> LoadUserList(string fileName)
        {
            Dictionary<int, User> returnDictionary = null;
            if (!File.Exists(GetDatabasePath(fileName)))
            {
                using FileStream createStream = File.Create(GetDatabasePath(fileName));
            }
            else
            {
                returnDictionary = JsonConvert.DeserializeObject<Dictionary<int, User>>(File.ReadAllText(GetDatabasePath(fileName)));
            }
            if (returnDictionary == null)
            {
                returnDictionary = new Dictionary<int, User>();
            }
            return returnDictionary;
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
