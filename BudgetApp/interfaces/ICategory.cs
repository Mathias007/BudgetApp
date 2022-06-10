using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface ICategory : ITransactionObject
    {
        int CategoryID { get; set; }
        string CategoryType { get; set; }
        string CategoryName { get; set; }
    }
}
