using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp
{
    public class Category : BudgetElement, ICategory
    {
        private string _type;
        private string _name;
        private bool _isActive;

        public int CategoryID { get => _id; set => _id = value; }
        public string CategoryType { get => _type; set => _type = value; }
        public string CategoryName { get => _name; set => _name = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public Category(int id, string type, string name, bool isActive = true)
        {
            _id = id;
            _type = type;
            _name = name;
            _isActive = isActive;
        }

        public override void PrintProperties()
        {
            Console.WriteLine($"id: {_id} \n" +
                $"categoryName: {_type} \n" +
                $"categoryType: {_name} \n" +
                $"isActive: {_isActive} \n");
        }
        public static void PrintCategories(bool onlyActive, Dictionary<int, Category> categoriesList)
        {
            foreach (KeyValuePair<int, Category> record in categoriesList)
            {
                if (!onlyActive || record.Value.IsActive)
                {
                    if (record.Value.CategoryType == "expense") Console.ForegroundColor = ConsoleColor.Red;
                    else if (record.Value.CategoryType == "income") Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine(
                            $" + {record.Key}: " +
                            $"{record.Value.CategoryName} ({record.Value.CategoryType})");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public static void AddNewCategory(Dictionary<int, Category> categoriesList)
        {
            Console.Clear();

            AnsiConsole.Write(new Rule("[yellow]Dodaj kategorię[/]"));

            int categoryID = categoriesList.Count == 0 ? 1 : (categoriesList.Keys.Max() + 1);

            var categoriesPrompt = new SelectionPrompt<string>()
                .PageSize(5)
                .Title("Wybierz, czy kategoria ma być [green]dochodem[/] (income), czy [red]wydatkiem[/] (expense) \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                .AddChoices(new[]{"dochód", "wydatek" });

            string categoryType = (AnsiConsole.Prompt(categoriesPrompt) == "dochód" ? "income" : "expense");
            AnsiConsole.MarkupLine("Wybrany typ: [yellow]{0}[/]", categoryType);

            string categoryName = AnsiConsole.Ask<string>("Wprowadź [green]nazwę kategorii[/]: ");
            Category addingCategory = new(categoryID, categoryType, categoryName);

            categoriesList.Add(addingCategory.CategoryID, addingCategory);

            AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

            AnsiConsole.Write(new Markup("\n [bold darkorange]Kategoria została pomyślnie dodana! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));

            Console.ReadKey();
            Console.Clear();
        }

        public static void EditExistingCategory(int selectedCategoryID, Dictionary<int, Category> categoriesList)
        {
            AnsiConsole.Write(new Rule("[yellow]Edytuj kategorię[/]"));

            var categoriesPrompt = new SelectionPrompt<string>()
                .PageSize(5)
                .Title("Wybierz, czy kategoria ma być [green]dochodem[/] (income), czy [red]wydatkiem[/] (expense) \n ([grey]Operuj strzałkami, a następnie naciśnij [green]ENTER[/] do zatwierdzenia)[/]")
                .AddChoices(new[] { "dochód", "wydatek" });

            string categoryType = (AnsiConsole.Prompt(categoriesPrompt) == "dochód" ? "income" : "expense");
            AnsiConsole.MarkupLine("Wybrany typ: [yellow]{0}[/]", categoryType);

            string categoryName = AnsiConsole.Ask("Wprowadź [green]nazwę kategorii[/]: ", categoriesList[selectedCategoryID].CategoryName);

            bool categoryIsActive = AnsiConsole.Confirm("Czy kategoria ma być aktywna?");            

            AnsiConsole.Write(new Rule("[yellow]Koniec[/]"));

            AnsiConsole.Write(new Markup("\n [bold darkorange]Kategoria została pomyślnie zedytowana! Naciśnij dowolny klawisz aby wrócić do menu.[/]"));

            Category newCategory = new(selectedCategoryID, categoryType, categoryName, categoryIsActive);

            categoriesList[selectedCategoryID] = newCategory;

            Console.ReadKey();
            Console.Clear();
        }

        public static void ManageCategories(Dictionary<int, Category> categoriesList)
        {
            Console.Clear();

            var categoriesTable = new Table();
            var font = FigletFont.Load(BudgetService.GetDatabasePath("assets/ogre.flf"));

            AnsiConsole.Write(
                     new FigletText(font, "Kategorie")
                    .Centered()
                    .Color(Color.Blue));

            categoriesTable
                .Border(TableBorder.Ascii)
                .AddColumn(new TableColumn("[darkorange][b]ID[/][/]").Footer("[darkorange][b]ID[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Typ[/][/]").Footer("[darkorange][b]Typ[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Nazwa[/][/]").Footer("[darkorange][b]Nazwa[/][/]").Centered())
                .AddColumn(new TableColumn("[darkorange][b]Aktywna[/][/]").Footer("[darkorange][b]Aktywna[/][/]").Centered());


            foreach (KeyValuePair<int, Category> category in categoriesList)
            {
                categoriesTable.AddRow(
                    category.Value.CategoryID.ToString(),
                    category.Value.CategoryType.ToString(),
                    $"{(category.Value.CategoryType == "income" ? "[green]" : "[red]")}{category.Value.CategoryName}[/]",
                    $"{(category.Value.IsActive ? "[blue]TAK[/]" : "[grey]NIE[/]")}"
                 );
            }

            AnsiConsole.Write(categoriesTable);

            AnsiConsole.Write(new Rule("[yellow]Opcje[/]").LeftAligned());

            Dictionary<ConsoleKey, string> categoriesOptions = new()
            {
                { ConsoleKey.W, "Dodaj nową kategorię" },
                { ConsoleKey.D, "Modyfikuj istniejącą kategorię" },
                { ConsoleKey.U, "Wróć do menu" }
            };

            ConsoleKey selector;

            var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($" \t\t\t\t [darkorange]Co chcesz zrobić?[/]")
                .PageSize(5)
                .MoreChoicesText("[grey](Przesuwaj w górę i w dół, a wybraną opcję zatwierdź klawiszem ENTER)[/]")
                .AddChoices(categoriesOptions.Values)
            );

            AnsiConsole.WriteLine($"Wybrałeś opcję: {selectedOption}");

            selector = categoriesOptions.FirstOrDefault(option => option.Value == selectedOption).Key;

            switch (selector)
            {
                case ConsoleKey.W:
                    AddNewCategory(categoriesList);
                    break;

                case ConsoleKey.D:
                    Console.Write("Podaj id wybranej kategorii: ");
                    string inputID = Console.ReadLine();
                    int selectedID = -1;
                    if (int.TryParse(inputID, out selectedID) && categoriesList.ContainsKey(selectedID))
                    {
                        EditExistingCategory(selectedID, categoriesList);
                        return;
                    }
                    Console.WriteLine("Brak kategorii zapisanej pod wybraną pozycją!");
                    break;

                case ConsoleKey.U:
                    Console.Clear();
                    return;

                default:
                    Console.Clear();
                    return;
            }
        }
    }
}
