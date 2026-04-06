using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Класс значений для настроек лабиринта и сетки.
    /// </summary>
    public class MazeSettings
    {
        // Размер сетки
        public int TilemapRows { get; }
        public int TilemapCols { get; }
        
        // Минимальные и максимальные значение размера сетки
        private const int MinTilemapRows = 5;
        private const int MaxTilemapRows = 100;
        private const int MinTilemapCols = 5;
        private const int MaxTilemapCols = 100;
        
        // Размер лабиринта
        public int MazeRows { get; }
        public int MazeCols { get; }
        
        // Минимальные значения размера лабиринта
        private const int MinMazeRows = 5;
        private const int MinMazeCols = 5;
        
        // Начальная позиция лабиринта на сетке
        public int MazeStartRow { get; }
        public int MazeStartCol { get; }
        
        // Минимальные значения начальных позиций лабирината на сетке
        private const int MinMazeStartRow = 0;
        private const int MinMazeStartCol = 0;

        // Радиус безопасной зоны (квадрат)
        public int SafeZoneSquareRadius { get; }
        
        // Минимальное и максимальное значение радиуса безопасной зоны
        private const int MinSafeZoneSquareRadius = 1;
        private const int MaxSafeZoneSquareRadius = 10;
        
        // Коэффициент для определения радиуса безопасной зоны
        private const float SafeZoneFactor = 0.2f;

        public MazeSettings(
            int tilemapRows,
            int tilemapCols,
            int mazeRows,
            int mazeCols,
            int mazeStartRow,
            int mazeStartCol,
            int safeZoneSquareRadius)
        {
            if (tilemapRows > MaxTilemapRows || tilemapRows < MinTilemapRows)
                throw new ArgumentOutOfRangeException(nameof(tilemapRows),
                    $"Expected {MinTilemapRows} <= TilemapRows <= {MaxTilemapRows}");
            
            if (tilemapCols > MaxTilemapCols || tilemapCols < MinTilemapCols)
                throw new ArgumentOutOfRangeException(nameof(tilemapCols),
                    $"Expected {MinTilemapCols} <= TilemapCols <= {MaxTilemapCols}");
            
            if (mazeStartRow > tilemapRows - MinMazeRows || mazeStartRow < MinMazeStartRow)
                throw new ArgumentOutOfRangeException(nameof(mazeStartRow),
                    "StartMazeRow must allow the maze to fit in the tilemap: " +
                    $"expected {MinMazeStartRow} <= StartMazeRow <= {tilemapRows - MinMazeRows}");
            
            if (mazeStartCol > tilemapCols - MinMazeCols || mazeStartCol < MinMazeStartCol)
                throw new ArgumentOutOfRangeException(nameof(mazeStartCol),
                    "StartMazeCol must allow the maze to fit in the tilemap: " +
                    $"expected {MinMazeStartCol} <= StartMazeCol <= {tilemapCols - MinMazeCols}");
            
            if (mazeRows > tilemapRows - mazeStartRow || mazeRows < MinMazeRows)
                throw new ArgumentOutOfRangeException(nameof(mazeRows),
                    "MazeRows must fit within the tilemap from the start row: " +
                    $"expected {MinMazeRows} <= MazeRows <= {tilemapRows - mazeStartRow}");
            
            if (mazeCols > tilemapCols - mazeStartCol || mazeCols < MinMazeCols)
                throw new ArgumentOutOfRangeException(nameof(mazeCols),
                    "MazeCols must fit within the tilemap from the start column: " +
                    $"expected {MinMazeCols} <= MazeCols <= {tilemapCols - mazeStartCol}");

            int maxAllowedRadius = Math.Min(
                MaxSafeZoneSquareRadius, 
                (int)(MathF.Min(mazeRows, mazeCols) * SafeZoneFactor));
            
            if (safeZoneSquareRadius > maxAllowedRadius || safeZoneSquareRadius < MinSafeZoneSquareRadius)
                throw new ArgumentOutOfRangeException(nameof(safeZoneSquareRadius),
                    $"Expected {MinSafeZoneSquareRadius} <= SafeZoneSquareRadius <= {maxAllowedRadius} " +
                    "(safe zone must fit inside the maze and not occupy the entire area)");

            TilemapRows = tilemapRows;
            TilemapCols = tilemapCols;
            MazeRows = mazeRows;
            MazeCols = mazeCols;
            MazeStartRow = mazeStartRow;
            MazeStartCol = mazeStartCol;
            SafeZoneSquareRadius = safeZoneSquareRadius;
        }
    }
}