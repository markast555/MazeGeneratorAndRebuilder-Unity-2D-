using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Настройки лабиринта
    /// </summary>
    public class MazeSettings
    {
        // Размер лабиринта
        public int MazeRows { get; }
        public int MazeCols { get; }
        
        // Минимальные и максимальные значения размера лабиринта
        public const int MinMazeRows = 5;
        public const int MaxMazeRows = 30;
        public const int MinMazeCols = 5;
        public const int MaxMazeCols = 30;
        
        // Размер лабиринта по умолчанию
        public const int DefaultMazeRows = 10;
        public const int DefaultMazeCols = 10;

        public MazeSettings(
            int mazeRows,
            int mazeCols
            )
        {
            
            if (mazeRows > MaxMazeRows || mazeRows < MinMazeRows)
                throw new ArgumentOutOfRangeException(nameof(mazeRows), mazeRows,
                    "MazeRows must fit within the tilemap from the start row: " +
                    $"expected {MinMazeRows} <= MazeRows <= {MaxMazeRows}");
            
            if (mazeCols > MaxMazeCols || mazeCols < MinMazeCols)
                throw new ArgumentOutOfRangeException(nameof(mazeCols), mazeCols,
                    "MazeCols must fit within the tilemap from the start column: " +
                    $"expected {MinMazeCols} <= MazeCols <= {MaxMazeCols}");

            
            MazeRows = mazeRows;
            MazeCols = mazeCols;
        }
    }
}