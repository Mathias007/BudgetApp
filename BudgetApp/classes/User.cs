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

        public static void PrintUsers(bool onlyActive, Dictionary<int, User> usersList)
        {
            foreach (KeyValuePair<int, User> record in usersList)
            {
                if (!onlyActive || record.Value.IsActive)
                {
                    Console.WriteLine(
                        $" + [{record.Key}]: " +
                        $"{record.Value.UserFirstName} {record.Value.UserLastName} " +
                        $"{(record.Value.UserIsActive ? "AKTYWNY" : "NIEAKTYWNY")} " +
                        $"{(record.Value.UserIsAdmin ? "ADMINISTRATOR" : "USER")} ");
                }
            }
        }

        public static void ManageUsers(Dictionary<int, User> usersList)
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich domowników:");
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(" + [0]: dodaj nowego domownika");
            PrintUsers(false, usersList);
            Console.WriteLine(" \n -> Wybierz 0, aby dodać nowego domownika. \n -> Jeżeli chcesz zmodyfikować dane istniejącego domownika, wypisz jego numer ID. \n -> Aby wrócić do głównego menu, naciśnij ENTER, pozostawiając pole puste.");
            int consoleID = GetConsoleInput<User>.GetUserInputID(usersList, false);
            if (consoleID == -1)
                Console.WriteLine(" + [0]: dodaj nowego domownika");
            foreach (KeyValuePair<int, User> record in usersList)
            {
                Console.WriteLine(
                    $" + [{record.Key}]: " +
                    $"{record.Value.UserFirstName} {record.Value.UserLastName} " +
                    $"{(record.Value.UserIsActive ? "AKTYWNY" : "NIEAKTYWNY")} " +
                    $"{(record.Value.UserIsAdmin ? "ADMINISTRATOR" : "USER")} ");
            }

            Console.WriteLine(" \n -> Wybierz 0, aby dodać nowego domownika. \n -> Jeżeli chcesz zmodyfikować dane istniejącego domownika, wypisz jego numer ID. \n -> Aby wrócić do głównego menu, naciśnij ENTER, pozostawiając pole puste.");
            string userInput = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(userInput))

            {
                Console.Clear();
                return;
            }
            if (consoleID == 0)
            {
                int newUserID = usersList.Count == 0 ? 1 : usersList.Keys.Max() + 1;
                Console.WriteLine("Imię: ");
                string firstName = Console.ReadLine();
                Console.WriteLine("Nazwisko: ");
                string lastName = Console.ReadLine();
                User addingUser = new(newUserID, firstName, lastName);
                usersList.Add(addingUser.UserID, addingUser);
                return;
            }
            Console.Clear();

            Console.WriteLine($"Wpisz nowe imię, zostaw puste żeby pominiąć({usersList[consoleID].UserFirstName}): ");
            string newFirstName = Console.ReadLine();
            usersList[consoleID].UserFirstName = String.IsNullOrWhiteSpace(newFirstName) ? usersList[consoleID].UserFirstName : newFirstName;
            Console.Clear();
            Console.WriteLine($"Wpisz nowe nazwisko, zostaw puste żeby pominiąć({usersList[consoleID].UserLastName}): ");
            string newLastName = Console.ReadLine();
            usersList[consoleID].UserLastName = String.IsNullOrWhiteSpace(newLastName) ? usersList[consoleID].UserLastName : newLastName;
            Console.Clear();
            Console.WriteLine($"Domownik jest aktywny({usersList[consoleID].UserIsActive})? (t/n), zostaw puste żeby nie zmieniać");
            string newActiveStatus = Console.ReadLine().ToUpper();

            if (newActiveStatus.Equals("T"))
                usersList[consoleID].UserIsActive = true;
            else if (newActiveStatus.Equals("N"))
                usersList[consoleID].UserIsActive = false;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }
    }
}
