using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    interface IUser
    {
        int UserID { get; set; }
        string UserFirstName { get; set; }
        string UserLastName { get; set; }
        bool UserIsAdmin { get; set; }
    }
}
