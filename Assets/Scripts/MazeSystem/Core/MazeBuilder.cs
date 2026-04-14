namespace MazeSystem.Core
{
    /// <summary>
    /// Строитель лабиринта
    /// </summary>
    public static class MazeBuilder
    {
        /// <summary>
        /// Инициализирует ячейки лабиринта и связывает их между собой.
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        public static void InitMaze(Maze maze)
        {
            CreateCells(maze);
            LinkNeighbors(maze);
        }

        /// <summary>
        /// Создаёт ячейки лабиринта.
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        private static void CreateCells(Maze maze)
        {
            for (int i = 0; i < maze.Rows; i++)
            {
                for (int j = 0; j < maze.Cols; j++)
                {
                    Cell cell = new Cell(i, j);
                    maze.SetCell(i, j, cell);
                    cell.AddWall(BorderSide.All);
                }
            }
        }

        /// <summary>
        /// Устанавливает соседние связи между ячейками.
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        private static void LinkNeighbors(Maze maze)
        {
            for (int i = 0; i < maze.Rows; i++)
            {
                for (int j = 0; j < maze.Cols; j++)
                {
                    var current = maze.GetCell(i, j);

                    if (i > 0)
                        current.Neighbors[BorderSide.Bottom] = maze.GetCell(i - 1, j);

                    if (i < maze.Rows - 1)
                        current.Neighbors[BorderSide.Top] = maze.GetCell(i + 1, j);

                    if (j > 0)
                        current.Neighbors[BorderSide.Left] = maze.GetCell(i, j - 1);

                    if (j < maze.Cols - 1)
                        current.Neighbors[BorderSide.Right] = maze.GetCell(i, j + 1);
                }
            }
        }
    }
}