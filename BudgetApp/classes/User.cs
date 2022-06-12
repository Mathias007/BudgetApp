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
        public static User createUser(Dictionary<int,User> userList)
        {
            int newUserID = userList.Count == 0 ? 1 : userList.Keys.Max() + 1;
            Console.WriteLine("Imię: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("Nazwisko: ");
            string lastName = Console.ReadLine();
            return new User(newUserID, firstName, lastName);
        }
        public static User editUser(User userToBeEdited)
        {
            Console.WriteLine($"Wpisz nowe imię, zostaw puste żeby pominiąć({userToBeEdited.UserFirstName}): ");
            string newFirstName = Console.ReadLine();
            userToBeEdited.UserFirstName = String.IsNullOrWhiteSpace(newFirstName) ? userToBeEdited.UserFirstName : newFirstName;
            Console.Clear();
            Console.WriteLine($"Wpisz nowe nazwisko, zostaw puste żeby pominiąć({userToBeEdited.UserLastName}): ");
            string newLastName = Console.ReadLine();
            userToBeEdited.UserLastName = String.IsNullOrWhiteSpace(newLastName) ? userToBeEdited.UserLastName : newLastName;
            Console.Clear();
            Console.WriteLine($"Domownik jest aktywny({userToBeEdited.UserIsActive})? (t/n), zostaw puste żeby nie zmieniać");
            string newActiveStatus = Console.ReadLine().ToUpper();
            if (newActiveStatus.Equals("T"))
                userToBeEdited.UserIsActive = true;
            else if (newActiveStatus.Equals("N"))
                userToBeEdited.UserIsActive = false;
            return userToBeEdited; //można przerobić żeby tworzyło nowego usera zamiast edytować
        }
    }
}