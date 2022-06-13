using System;
using System.Collections.Generic;

namespace BudgetApp
{
    public interface ITransactionObject
    {
        bool IsActive { get; set; }
    }
}
