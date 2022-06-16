using System;
using System.Collections.Generic;

namespace BudgetApp
{
    interface IBudgetMenu
    {
        bool IsProgramOpen { get; set; }
        Dictionary<ConsoleKey, string> ProgramOptions { get; }
        ConsoleKey OptionSelector { get; set; }

        void HandleMenu(User user);
        static void ExitFromProgram() => throw new NotImplementedException();
    }
}
