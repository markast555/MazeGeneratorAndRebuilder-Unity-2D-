using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Настройки квадратной безопасной зоны
    /// </summary>
    public class SquareSafeZoneSettings: ISafeZoneSettings
    {
        public SafeZoneMode Mode => SafeZoneMode.Square;
        
        /// <summary>
        /// Радиус безопасной зоны в ячейках (клетках)
        /// </summary>
        public int Radius { get; }
        
        // Минимальное и максимальное значения радиуса безопасной зоны
        public const int MinRadius = 1;
        public const int MaxRadius = 10;
        
        // Радиус безопасной зоны по умолчанию
        public const int DefaultRadius = 2;
        
        // Коэффициент масштабирования безопасной зоны
        public const float RadiusFactor = 0.2f;

        /// <summary>
        /// Создаёт настройки квадратной безопасной зоны
        /// </summary>
        /// <param name="radius">Радиус безопасной зоны</param>
        /// <param name="mazeRows">Количество строк в лабиринте</param>
        /// <param name="mazeCols">Количество столбцов в лабиринте</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрано значение радиуса, которое не помещается в размеры лабиринта
        /// или выходит за допустимые границы
        /// </exception>
        public SquareSafeZoneSettings(
            int radius,
            int mazeRows,
            int mazeCols
        )
        {
            int maxAllowedRadius = SafeZoneCalculator.CalculateMax(
                mazeRows,
                mazeCols,
                MinRadius,
                MaxRadius,
                RadiusFactor
            );
            
            if (radius > maxAllowedRadius || radius < MinRadius)
                throw new ArgumentOutOfRangeException(nameof(radius), radius,
                    $"Expected {MinRadius} <= Radius <= {maxAllowedRadius} " +
                    "(safe zone must fit inside the maze and not occupy the entire area)");
            
            Radius = radius;
        }
        
    }
}