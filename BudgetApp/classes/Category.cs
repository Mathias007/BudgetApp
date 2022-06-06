using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class Category : ICategory
    {
        private int _id;
        private string _type;
        private string _name;

        public int CategoryID { get => _id; set => _id = value; }
        public string CategoryType { get => _type; set => _type = value; }
        public string CategoryName { get => _name; set => _name = value; }

        public Category(int id, string type, string name)
        {
            _id = id;
            _type = type;
            _name = name;
        }
    }
}
