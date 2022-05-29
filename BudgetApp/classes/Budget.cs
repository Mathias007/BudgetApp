using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    class Budget : BudgetService, IBudget
    {
        public string SelectedMonth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Balance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double[] BudgetStructure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
