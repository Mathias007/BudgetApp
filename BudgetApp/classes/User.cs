using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    class User : IUser
    {
        private string _firstName;
        private string _lastName;
        private bool _isActive;

        public string UserFirstName { get => _firstName; set => _firstName = value; }
        public string UserLastName { get => _lastName; set => _lastName = value; }
        public bool UserIsActive { get => _isActive; set => _isActive = value; }


        public User(string firstName, string lastName, bool isActive)
        {
            _firstName = firstName;
            _lastName = lastName;
            _isActive = isActive;
        }
    }
}
