using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class Budget : BudgetService, IBudget
    {
        public Dictionary<int, Transaction> _budget;
        public double _balance = 0;

        public Dictionary<int, Transaction> BudgetData { get => _budget; set => _budget = value; }
        public string BudgetSelector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double BudgetBalance { get => _balance; set => _balance = value; }
        public double[] BudgetStructure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Budget(Dictionary<int, Transaction> initialBudget)
        {
           _budget = initialBudget;
        }

        public void UpdateBudget(Dictionary<int, Transaction> newData) => _budget = newData;

        public void CalculateBalance() {
            foreach (KeyValuePair<int, Transaction> record in _budget) _balance += record.Value.TransactionAmount;

            Console.WriteLine($"Stan konta: {_balance}");
            _balance = 0;
        }

    }


}
