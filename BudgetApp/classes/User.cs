using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class User : IUser
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private bool _isActive;
        private bool _isAdmin;

        public int UserID { get => _id; set => _id = value; }
        public string UserFirstName { get => _firstName; set => _firstName = value; }
        public string UserLastName { get => _lastName; set => _lastName = value; }
        public bool UserIsActive { get => _isActive; set => _isActive = value; }
        public bool UserIsAdmin { get => _isAdmin; set => _isAdmin = value; }

        public User(int id, string firstName, string lastName, bool isActive, bool isAdmin)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
            _isActive = isActive;
            _isAdmin = isAdmin;
        }
        public static User addUser(int maxId)
        {
            Console.WriteLine("Imię: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("Nazwisko: ");
            string lastName = Console.ReadLine();
            return new User(maxId + 1, firstName, lastName, true, false);
        }
    }
}
