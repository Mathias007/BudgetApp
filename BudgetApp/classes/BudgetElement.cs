using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class BudgetElement
    {
        public int _id { get; set; }

        public virtual void PrintProperties() => Console.WriteLine($"id: {_id}");
    }
}
