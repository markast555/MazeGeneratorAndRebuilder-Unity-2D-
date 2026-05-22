namespace MazeSystem.Core
{
    /// <summary>
    /// Карта безопасной зоны лабиринта
    /// </summary>
    public class SafeZone
    {
        private readonly bool[,] _zone;
        public int Rows => _zone.GetLength(0);
        public int Cols => _zone.GetLength(1);

        public SafeZone(int rows, int cols)
        {
            _zone = new bool[rows, cols];
        }

        /// <summary>
        /// Устанавливает принадлежность ячейки к безопасной зоне
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="col">Столбец</param>
        /// <param name="value">Значение</param>
        public void SetSafe(int row, int col, bool value)
        {
            _zone[row, col] = value;
        }

        /// <summary>
        /// Проверяет, входит ли ячейка в безопасную зону
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="col">Столбец</param>
        /// <returns>Значение</returns>
        public bool IsSafe(int row, int col)
        {
            return _zone[row, col];
        }
    }
}