using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface IBudget
    {
        double BudgetBalance { get; set; }
        Dictionary<string, (double CategoryAmount, double CategoryPercentage)> BudgetStructure { get; set; }

        Dictionary<int, Transaction> TransactionsList { get; set; }
        Dictionary<int, User> UsersList { get; set; }
        Dictionary<int, Category> CategoriesList { get; set; }

    }
}
