using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace BudgetApp
{
    class Spectre
    {
        // CHART EXAMPLE

        public static void ChartExample()
        {
            // Render a bar chart
            AnsiConsole.WriteLine();
            Render("Fruits per month", new BarChart()
                .Width(60)
                .Label("[green bold underline]Number of fruits[/]")
                .CenterLabel()
                .AddItem("Apple", 12, Color.Yellow)
                .AddItem("Orange", 54, Color.Green)
                .AddItem("Banana", 33, Color.Red));

            // Render a breakdown chart
            AnsiConsole.WriteLine();
            Render("Languages used", new BreakdownChart()
                .FullSize()
                .Width(60)
                .ShowPercentage()
                .AddItem("SCSS", 37, Color.Red)
                .AddItem("HTML", 28.3, Color.Blue)
                .AddItem("C#", 22.6, Color.Green)
                .AddItem("JavaScript", 6, Color.Yellow)
                .AddItem("Ruby", 6, Color.LightGreen)
                .AddItem("Shell", 0.1, Color.Aqua));
        }

        private static void Render(string title, IRenderable chart)
        {
            AnsiConsole.Write(
                new Panel(chart)
                    .Padding(1, 1)
                    .Header(title));
        }


        // TABLE EXAMPLE

        public static void TableCreator()
        {
            AnsiConsole.Write(CreateTable());
        }

        private static Table CreateTable()
        {
            var simple = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Red)
                .AddColumn(new TableColumn("[u]CDE[/]").Footer("EDC").Centered())
                .AddColumn(new TableColumn("[u]FED[/]").Footer("DEF"))
                .AddColumn(new TableColumn("[u]IHG[/]").Footer("GHI"))
                .AddRow("Hello", "[red]World![/]", "")
                .AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]")
                .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            var second = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green)
                .AddColumn(new TableColumn("[u]Foo[/]"))
                .AddColumn(new TableColumn("[u]Bar[/]"))
                .AddColumn(new TableColumn("[u]Baz[/]"))
                .AddRow("Hello", "[red]World![/]", "")
                .AddRow(simple, new Text("Whaaat"), new Text("Lolz"))
                .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            return new Table()
                .Centered()
                .Border(TableBorder.DoubleEdge)
                .Title("TABLE [yellow]TITLE[/]")
                .Caption("TABLE [yellow]CAPTION[/]")
                .AddColumn(new TableColumn(new Panel("[u]ABC[/]").BorderColor(Color.Red)).Footer("[u]FOOTER 1[/]"))
                .AddColumn(new TableColumn(new Panel("[u]DEF[/]").BorderColor(Color.Green)).Footer("[u]FOOTER 2[/]"))
                .AddColumn(new TableColumn(new Panel("[u]GHI[/]").BorderColor(Color.Blue)).Footer("[u]FOOTER 3[/]"))
                .AddRow(new Text("Hello").Centered(), new Markup("[red]World![/]"), Text.Empty)
                .AddRow(second, new Text("Whaaat"), new Text("Lol"))
                .AddRow(new Markup("[blue]Hej[/]").Centered(), new Markup("[yellow]Världen![/]"), Text.Empty);
        }

        // LIVE TABLE EXAMPLE

        private const int NumberOfRows = 10;

        private static readonly Random _random = new();
        private static readonly string[] _exchanges = new string[]
        {
            "SGD", "SEK", "PLN",
            "MYR", "EUR", "USD",
            "AUD", "JPY", "CNH",
            "HKD", "CAD", "INR",
            "DKK", "GBP", "RUB",
            "NZD", "MXN", "IDR",
            "TWD", "THB", "VND",
        };

        public static async Task LiveTableExample()
        {
            var table = new Table().Expand().BorderColor(Color.Grey);
            table.AddColumn("[yellow]Source currency[/]");
            table.AddColumn("[yellow]Destination currency[/]");
            table.AddColumn("[yellow]Exchange rate[/]");

            AnsiConsole.MarkupLine("Press [yellow]CTRL+C[/] to exit");

            await AnsiConsole.Live(table)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Cropping(VerticalOverflowCropping.Bottom)
                .StartAsync(async ctx =>
                {
                // Add some initial rows
                foreach (var _ in Enumerable.Range(0, NumberOfRows))
                    {
                        AddExchangeRateRow(table);
                    }

                // Continously update the table
                while (true)
                    {
                    // More rows than we want?
                    if (table.Rows.Count > NumberOfRows)
                        {
                        // Remove the first one
                        table.Rows.RemoveAt(0);
                        }

                    // Add a new row
                    AddExchangeRateRow(table);

                    // Refresh and wait for a while
                    ctx.Refresh();
                        await Task.Delay(400);
                    }
                });
        }

        private static void AddExchangeRateRow(Table table)
        {
            var (source, destination, rate) = GetExchangeRate();
            table.AddRow(
                source, destination,
                _random.NextDouble() > 0.35D ? $"[green]{rate}[/]" : $"[red]{rate}[/]");
        }

        private static (string Source, string Destination, double Rate) GetExchangeRate()
        {
            var source = _exchanges[_random.Next(0, _exchanges.Length)];
            var dest = _exchanges[_random.Next(0, _exchanges.Length)];
            var rate = 200 / ((_random.NextDouble() * 320) + 1);

            while (source == dest)
            {
                dest = _exchanges[_random.Next(0, _exchanges.Length)];
            }

            return (source, dest, rate);
        }


            public static void Hello()
        {
            AnsiConsole.Markup("[underline red]Hello[/] World! \n");
        }

        public static void PrintLiveTable()
        {
            var table = new Table().Centered();

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    table.AddColumn("Foo");
                    ctx.Refresh();
                    Thread.Sleep(1000);

                    table.AddColumn("Bar");
                    ctx.Refresh();
                    Thread.Sleep(1000);
                });
        }

        public static void RenderTree()
        {
            var root = new Tree("Root");

            // Add some nodes
            var foo = root.AddNode("[yellow]Foo[/]");
            var table = foo.AddNode(new Table()
                .RoundedBorder()
                .AddColumn("First")
                .AddColumn("Second")
                .AddRow("1", "2")
                .AddRow("3", "4")
                .AddRow("5", "6"));

            table.AddNode("[blue]Baz[/]");
            foo.AddNode("Qux");

            var bar = root.AddNode("[yellow]Bar[/]");
            bar.AddNode(new Calendar(2020, 12)
        .AddCalendarEvent(2020, 12, 12)
        .HideHeader());

            // Render the tree
            AnsiConsole.Write(root);
        }
    }
}
