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
        public const int MinTilemapRows = 5;
        public const int MaxTilemapRows = 100;
        public const int MinTilemapCols = 5;
        public const int MaxTilemapCols = 100;
        
        // Размер сетки по умолчанию
        public const int DefaultTilemapRows = 14;
        public const int DefaultTilemapCols = 14;
        
        
        // Начальная позиция лабиринта на сетке
        public int MazeStartRow { get; }
        public int MazeStartCol { get; }
        
        // Минимальные значения начальных позиций лабирината на сетке
        public const int MinMazeStartRow = 0;
        public const int MinMazeStartCol = 0;
        
        // Начальная позиция лабиринта на сетке по умолчанию
        public const int DefaultMazeStartRow = 2;
        public const int DefaultMazeStartCol = 2;
        
        
        // Размер лабиринта
        public int MazeRows { get; }
        public int MazeCols { get; }
        
        // Минимальные значения размера лабиринта
        public const int MinMazeRows = MinTilemapRows;
        public const int MinMazeCols = MinTilemapCols;
        
        // Размер лабиринта по умолчанию
        public const int DefaultMazeRows = 10;
        public const int DefaultMazeCols = 10;
        

        // Радиус безопасной зоны (квадрат)
        public int SafeZoneSquareRadius { get; }
        
        // Минимальное и максимальное значение радиуса безопасной зоны
        public const int MinSafeZoneSquareRadius = 1;
        public const int MaxSafeZoneSquareRadius = 10;
        
        // Радиус безопасной зоны (квадрат) по умолчанию
        public const int DefaultSafeZoneSquareRadius = 2;
        
        // Коэффициент для определения радиуса безопасной зоны
        public const float SafeZoneFactor = 0.2f;

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
                Math.Max(MinSafeZoneSquareRadius, 
                    (int)(Math.Min(mazeRows, mazeCols) * SafeZoneFactor)));
            
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