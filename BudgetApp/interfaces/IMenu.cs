using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface IMenu
    {
        bool IsProgramOpen { get; set; }
        Dictionary<string,string> ProgramOptions { get; }
        void HandleMenu(User user);
        static void ManageProgramWorking() => throw new NotImplementedException();
    }
}
