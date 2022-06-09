using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public interface ITransactionObject
    {
        bool IsActive { get; set; }
    }
}
