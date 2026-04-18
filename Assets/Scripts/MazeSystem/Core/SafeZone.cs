namespace MazeSystem.Core
{
    public class SafeZone
    {
        private bool[,] _zone;
        public int Rows => _zone.GetLength(0);
        public int Cols => _zone.GetLength(1);

        public SafeZone(int rows, int cols)
        {
            _zone = new bool[rows, cols];
        }

        internal void SetSafe(int row, int col, bool value)
        {
            _zone[row, col] = value;
        }

        public bool IsSafe(int row, int col)
        {
            return _zone[row, col];
        }
    }
}