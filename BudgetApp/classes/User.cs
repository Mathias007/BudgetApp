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
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public User(int id, string firstName, string lastName, bool isActive = true, bool isAdmin = false)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
            _isActive = isActive;
            _isAdmin = isAdmin;
        }
        private static User addUser(int maxId) //do usunięcia
        {
            Console.WriteLine("Imię: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("Nazwisko: ");
            string lastName = Console.ReadLine();
            return new User(maxId + 1, firstName, lastName, true, false);
        }
    }
}
