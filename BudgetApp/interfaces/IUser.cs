using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface IUser : ITransactionObject
    {
        int UserID { get; set; }
        string UserFirstName { get; set; }
        string UserLastName { get; set; }
        bool UserIsAdmin { get; set; }

        static void PrintUsers(bool onlyActive, Dictionary<int, User> usersList) => throw new NotImplementedException();
        static void ManageUsers(Dictionary<int, User> usersList) => throw new NotImplementedException();
    }
}
