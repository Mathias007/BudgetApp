using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class Category : ICategory
    {
        private string _type;
        private string _name;

        public string CategoryType { get => _type; set => _type = value; }
        public string CategoryName { get => _name; set => _name = value; }

        public Category(string type, string name)
        {
            _type = type;
            _name = name;
        }
    }
}
