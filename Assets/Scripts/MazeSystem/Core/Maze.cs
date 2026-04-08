using System.Collections.Generic;

namespace MazeSystem.Core
{
    /// <summary>
    /// Класс лабиринта.
    /// </summary>
    public class Maze
    {
        public int Rows => _maze.GetLength(0);
        public int Cols => _maze.GetLength(1);
        private readonly Cell[,] _maze;
        
        public Maze(int rows, int cols)
        {
            _maze = new Cell[rows, cols];
        }

        /// <summary>
        /// Получает ячейку.
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="col">Столбец</param>
        /// <returns>Ячейка</returns>
        public Cell GetCell(int row, int col)
        {
            return _maze[row, col];
        }

        /// <summary>
        /// Устанавливает ячейку.
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="col">Столбец</param>
        /// <param name="cell">Ячейка</param>
        public void SetCell(int row, int col, Cell cell)
        {
            _maze[row, col] = cell;
        }

        /// <summary>
        /// Возвращает последовательность всех ячеек лабиринта.
        /// </summary>
        /// <returns>Последовательность ячеек</returns>
        public IEnumerable<Cell> AllCells()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    yield return _maze[i, j];
                }
            }
        }
    }
}