namespace pvusGameOfLifeSequential
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int probabilityOfLife = 8; // 8% chance of a cell being alive

            // create a new game with 100 columns and 25 rows
            GameOfLife game = new GameOfLife(100, 25, probabilityOfLife);

            // initialize the game loop variables
            int generation = 1;
            long requiredMilliseconds = 0;
            long totalRequiredMilliseconds = 0;
            bool exit = false;

            // game loop
            while(!exit)
            {
                int avgRequiredMilliseconds = (int)(totalRequiredMilliseconds / generation);

                // clear the console and print the current generation
                Console.Clear();
                Console.WriteLine($"pvusGameOfLife - Generation {generation} ({requiredMilliseconds} ms / avg: {avgRequiredMilliseconds} ms) - X to stop / R to re-init\n");
                PrintGame(game);

                // calculate the next generation
                requiredMilliseconds = game.NextGeneration();
                totalRequiredMilliseconds += requiredMilliseconds;

                // wait for max 1 second
                Thread.Sleep(CalculateWaitTime(requiredMilliseconds));

                // check if the user wants to exit the game
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.X:
                            // signal the game loop to exit
                            exit = true;
                            break;
                        case ConsoleKey.R:
                            
                            game.InitializeGame(probabilityOfLife);
                            break;
                        default:
                            // do nothing when any other key is pressed
                            break;
                    }
                }

                generation++;
            }

            // print a message and wait for a key press before exiting
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Outputs the current state of the game to the console.
        /// </summary>
        /// <param name="game">The game.</param>
        private static void PrintGame(GameOfLife game)
        {
            for (int y = 0; y < game.GetHeight(); y++)
            {
                for (int x = 0; x < game.GetWidth(); x++)
                {
                    Console.Write(game.GetCell(x, y) ? "X" : ".");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Calculates the wait time in milliseconds with a maximum of 1 second.
        /// </summary>
        /// <param name="elapsedMilliseconds">The number of milliseconds it took to calculate the next generation.</param>
        /// <returns>The wait time left to wait one second.</returns>
        private static int CalculateWaitTime(long elapsedMilliseconds)
        {
            // calculate the wait time in milliseconds
            long waitTime = 1000 - elapsedMilliseconds;

            // do not wait if the calculation took longer than 1 second
            if (waitTime < 0)
            {
                waitTime = 0;
            }
            return (int)waitTime;
        }
    }
}