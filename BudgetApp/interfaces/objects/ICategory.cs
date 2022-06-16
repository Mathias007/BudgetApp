using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface ICategory : ITransactionObject
    {
        int CategoryID { get; set; }
        string CategoryType { get; set; }
        string CategoryName { get; set; }

        static void PrintCategories(bool onlyActive, Dictionary<int, Category> categoriesList) => throw new NotImplementedException();
        static void AddNewCategory(Dictionary<int, Category> categoriesList) => throw new NotImplementedException();
        static void EditExistingCategory(int selectedCategoryID, Dictionary<int, Category> categoriesList) => throw new NotImplementedException();
        static void ManageCategories(Dictionary<int, Category> categoriesList) => throw new NotImplementedException();
    }
}
