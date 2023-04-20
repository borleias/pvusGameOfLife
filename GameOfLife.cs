using System.Diagnostics;

namespace pvusGameOfLifeSequential
{
    public class GameOfLife
    {
        private int width;
        private int height;
        private bool[,] cells;

        /// <summary>
        /// Creates a new game with the given width and height and initializes the cells with a random state according to the given probability of life.
        /// </summary>
        /// <param name="width">The number of columns.</param>
        /// <param name="height">The number of rows.</param>
        /// <param name="probabilityOfLife">The probability of life in percent (1-100).</param>
        public GameOfLife(int width, int height, int probabilityOfLife)
        {
            this.SetWidth(width);
            this.SetHeight(height);
            this.InitializeGame(probabilityOfLife);
        }

        /// <summary>
        /// Returns the width of the game field.
        /// </summary>
        /// <returns>The number of columns.</returns>
        public int GetWidth() { return width; }

        /// <summary>
        /// Sets the width of the game field.
        /// </summary>
        /// <param name="width">A positive number of columns larger than zero.</param>
        private void SetWidth(int width)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "The number of columns must be larger than zero!");
            }

            this.width = width;
        }

        /// <summary>
        /// Returns the height of the game field.
        /// </summary>
        /// <returns>The number of rows.</returns>
        public int GetHeight() { return height; }

        /// <summary>
        /// Sets the height of the game field.
        /// </summary>
        /// <param name="height">A positive number of rows larger than zero.</param>
        private void SetHeight(int height)
        {
            if (height < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "The number of rows must be larger than zero!");
            }

            this.height = height;
        }

        /// <summary>
        /// Returns the array of cells.
        /// </summary>
        /// <returns>The array of cells.</returns>
        public bool[,] GetCells() { return cells; }

        /// <summary>
        /// Replaces all cells with the given array.
        /// </summary>
        /// <param name="value">The new array of cells.</param>
        public void SetCells(bool[,] value) { this.cells = value; }

        /// <summary>
        /// Sets the alive state of a cell for the given coordinates.
        /// </summary>
        /// <param name="x">The column index.</param>
        /// <param name="y">The row index.</param>
        /// <param name="value">True if the cell is alive; otherwise False.</param>
        public void SetCell(int x, int y, bool value)
        {
            this.GetCells()[x, y] = value;
        }

        /// <summary>
        /// Gets the alive state of a cell for the given coordinates.
        /// </summary>
        /// <param name="x">The column index.</param>
        /// <param name="y">The row index.</param>
        /// <returns>Returns True if alive; otherwise False.</returns>
        public bool GetCell(int x, int y)
        {
            return this.GetCells()[x, y];
        }

        /// <summary>
        /// Get the value of a cell (0 or 1) for the given coordinates.
        /// </summary>
        /// <param name="x">The column index.</param>
        /// <param name="y">The row index.</param>
        /// <returns>Returns 1 if alive; otherwise 0.</returns>
        public int GetCellValue(int x, int y)
        {
            return this.GetCells()[x, y] ? 1 : 0;
        }

        /// <summary>
        /// Calculate the next generation of cells.
        /// </summary>
        /// <returns>The number of milliseconds required for the calculation.</returns>
        public long NextGeneration()
        {
            // start a stopwatch to measure the time required for the calculation
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // create a new array to store the new generation
            bool[,] newCells = this.CalculateNextGeneration();

            // stop the stopwatch
            stopwatch.Stop();

            // overwrite the current generation with the new generation
            this.SetCells(newCells);

            // return the number of milliseconds required for the calculation
            return stopwatch.ElapsedMilliseconds;
        }

        private bool[,] CalculateNextGeneration()
        {
            // create a new array to store the new generation
            bool[,] newCells = new bool[this.GetWidth(), this.GetHeight()];

            int minX1 = 0;
            int minX2 = this.GetWidth() / 2;
            int maxX1 = this.GetWidth() / 2;
            int maxX2 = this.GetWidth();

            //CalculateNextGeneration(0, this.GetWidth() - 1, newCells);
            
            //CalculateNextGeneration(minX1, maxX1, newCells);
            //CalculateNextGeneration(minX2, maxX2, newCells);

            Thread t1 = new Thread(() => CalculateNextGeneration(minX1, maxX1, newCells));
            Thread t2 = new Thread(() => CalculateNextGeneration(minX2, maxX2, newCells));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            return newCells;
        }

        private void CalculateNextGeneration(int minX, int maxX, bool[,] game)
        {
            // loop through all cells
            for (int x = minX; x < maxX; x++)
            {
                for (int y = 0; y < this.GetHeight(); y++)
                {
                    // get the current state of the cell
                    bool cellState = this.GetCells()[x, y];

                    // count the number of neighbors
                    int neighbors = this.CountNeighbors(x, y);

                    // apply the rules of the game
                    if (neighbors == 3)
                    {
                        // cell is born or stays alive
                        cellState = true;
                    }
                    else if (neighbors == 2)
                    {
                        // do not change the state
                    }
                    else
                    {
                        // cell dies
                        cellState = false;
                    }

                    // set the new state of the cell
                    game[x, y] = cellState;
                }
            }
        }

        /*
        private bool[,] CalculateNextGeneration()
        {
            // create a new array to store the new generation
            bool[,] newCells = new bool[this.GetWidth(), this.GetHeight()];

            // loop through all cells
            for (int x = 0; x < this.GetWidth(); x++)
            {
                for (int y = 0; y < this.GetHeight(); y++)
                {
                    // get the current state of the cell
                    bool cellState = this.GetCells()[x, y];

                    // count the number of neighbors
                    int neighbors = this.CountNeighbors(x, y);

                    // apply the rules of the game
                    if (neighbors == 3)
                    {
                        // cell is born or stays alive
                        cellState = true;
                    }
                    else if (neighbors == 2)
                    {
                        // do not change the state
                    }
                    else
                    {
                        // cell dies
                        cellState = false;
                    }

                    // set the new state of the cell
                    newCells[x, y] = cellState;
                }
            }

            return newCells;
        }
         */

        /// <summary>
        /// Count the number of living neighbors for a given cell
        /// </summary>
        /// <param name="x">The column index.</param>
        /// <param name="y">The row index.</param>
        /// <returns>The number of living neighbors.</returns>
        private int CountNeighbors(int x, int y)
        {
            int neighbors =
                this.GetCellValue(this.Left(x), this.Above(y)) +
                this.GetCellValue(this.Left(x), y) +
                this.GetCellValue(this.Left(x), this.Below(y)) +
                this.GetCellValue(x, this.Above(y)) +
                this.GetCellValue(x, this.Below(y)) +
                this.GetCellValue(this.Right(x), this.Above(y)) +
                this.GetCellValue(this.Right(x), y) +
                this.GetCellValue(this.Right(x), this.Below(y));

            // simulate a long running calculation
            Thread.SpinWait(10000);

            return neighbors;
        }

        // helper methods to wrap around the edges of the grid

        /// <summary>
        /// Gets the column index of the cell to the left of the given cell.
        /// Wraps around the grid if the cell is on the left edge.
        /// </summary>
        /// <param name="x">The current column.</param>
        /// <returns>The index of the left column.</returns>
        private int Left(int x) { return x == 0 ? this.GetWidth() - 1 : x - 1; }

        /// <summary>
        /// Gets the column index of the cell to the right of the given cell.
        /// Wraps around the grid if the cell is on the right edge.
        /// </summary>
        /// <param name="x">The current column.</param>
        /// <returns>The index of the right column.</returns>
        private int Right(int x) { return (x + 1) % this.GetWidth(); }

        /// <summary>
        /// Gets the row index of the cell above the given cell.
        /// Wraps around the grid if the cell is on the top edge.
        /// </summary>
        /// <param name="y">The current row.</param>
        /// <returns>The index of the row above.</returns>
        private int Above(int y) { return y == 0 ? this.GetHeight() - 1 : y - 1; }

        /// <summary>
        /// Gets the row index of the cell below the given cell.
        /// Wraps around the grid if the cell is on the bottom edge.
        /// </summary>
        /// <param name="y">The current row.</param>
        /// <returns>The index of the row below.</returns>
        private int Below(int y) { return (y + 1) % this.GetHeight(); }

        public void InitializeGame(int probabilityOfLife)
        {
            // check if the probability of life is valid
            if (probabilityOfLife < 1 || probabilityOfLife > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(probabilityOfLife), "The probability of life must be a value between 1 and 100 percent.");
            }

            // create a random number generator
            Random rnd = new Random();

            // create a new array to store the new generation
            bool[,] newCells = new bool[this.GetWidth(), this.GetHeight()];

            // loop through all cells
            for (int x = 0; x < this.GetWidth(); x++)
            {
                for (int y = 0; y < this.GetHeight(); y++)
                {
                    int random = rnd.Next(1, 101);

                    if (random <= probabilityOfLife)
                    {
                        // cell is alive
                        newCells[x, y] = true;
                    }
                    else
                    {
                        // cell is dead
                        newCells[x, y] = false;
                    }
                }
            }

            this.SetCells(newCells);
        }
    }
}