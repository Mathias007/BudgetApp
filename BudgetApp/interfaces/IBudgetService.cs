using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface IBudgetService
    {
        string GetDatabasePath();
        void CreateBudgetDB();
        Task ShowTransactionList();
        double CalculateBalanceValue();
        double CalculateBudgetStructure();
    }
}
