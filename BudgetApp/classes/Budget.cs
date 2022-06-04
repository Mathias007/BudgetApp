using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    class Budget : BudgetService, IBudget
    {
        public Dictionary<int, Transaction> BudgetData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string BudgetSelector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double BudgetBalance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double[] BudgetStructure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
