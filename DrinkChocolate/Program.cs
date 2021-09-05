using System;

namespace DrinkChocolate
{
    class Program
    {

        private static PersistantData Data { get; set; }
        static void Main(string[] args)
        {
            Data = new PersistantData();
            Data.LoadData();

            // how many drinks can i have per week?
            Data.MaxDrinks = 5;

            //Simulate running program
            bool running = true;
            while (running)
            {
                var line = Console.ReadLine();

                switch (line)
                {
                    case "q":
                        running = false;
                        break;
                    case "d":
                        CanDrink(Data.MaxDrinks);
                        break;
                    case "a":
                        Data.AddDrink();
                        Console.WriteLine("Added one drnk");
                        break;
                    case "w":
                        // get time since last drink
                        Console.WriteLine(Data.GetTimneSinceLastDrink());
                        break;
                    case "c":
                        Data.ClearList();
                        Console.WriteLine("Cleared Drinks");
                        break;
                    case "u":
                        // undo last addition
                        Data.UndoLastAdd();
                        break;
                    case "r":
                        Data.RedoLastAdd();
                        break;
                    case "count":
                        // print how many you have been drinking
                        Console.WriteLine(Data.DrinkList.Count);
                        break;
                    default:
                        break;
                }

                if (line.Contains("set"))
                {
                    var parts = line.Split(" ");
                    int.TryParse(parts[1], out int newMax);
                    Data.MaxDrinks = newMax;
                }
            }
        }

        private static void CanDrink(int numDrinks)
        {
            // make a randomizer that gives either a yes or no.
            // the closer you get to the limit, the more no you get.
            Random ran = new Random();

            int ranRange = ran.Next(100) + 1;

            float multiplier = 100 / numDrinks;

            float noRange = multiplier * Data.DrinkList.Count;

            if (ranRange > noRange)
            {
                Console.WriteLine("Yes");
            }
            else
            {
                Console.WriteLine("No");
            }
        }
    }
}
