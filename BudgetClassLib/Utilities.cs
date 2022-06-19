using System;
using System.IO;

namespace UtilityLibraries
{
    public class UtilitiesLibrary
    {
        public static string GetDatabasePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\{fileName}"));
        }

        public static int RandomizeNumber(int min, int max) => new Random().Next(min, max);
    }
}
