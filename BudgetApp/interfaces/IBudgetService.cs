using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface IBudgetService
    {
        public Dictionary<int, Transaction> LoadDataFromDB();
        public Dictionary<int, Transaction> SaveDataToDB();
        double CalculateBalanceValue();
        double CalculateBudgetStructure();
    }
}
