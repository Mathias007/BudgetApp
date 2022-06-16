using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface IBudget
    {
        double BudgetBalance { get; set; }
        public Dictionary<string, double> IncomeStructure { get; set; }
        public Dictionary<string, double> ExpenseStructure { get; set; }

        Dictionary<int, Transaction> TransactionsList { get; set; }
        Dictionary<int, User> UsersList { get; set; }
        Dictionary<int, Category> CategoriesList { get; set; }

    }
}
