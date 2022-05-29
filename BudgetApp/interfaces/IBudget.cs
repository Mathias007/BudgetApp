using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface IBudget
    {
        string SelectedMonth { get; set; } 
        double Balance { get; set; }
        double[] BudgetStructure { get; set; }

    }
}
