using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface IBudget
    {
        Dictionary<int,Transaction> BudgetData { get; set; }
        string BudgetSelector { get; set; } 
        double BudgetBalance { get; set; }
        Dictionary<string, (double CategoryAmount, double CategoryPercentage)> BudgetStructure { get; set; }

    }
}
