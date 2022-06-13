using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface ICategory : ITransactionObject
    {
        int CategoryID { get; set; }
        string CategoryType { get; set; }
        string CategoryName { get; set; }
    }
}
