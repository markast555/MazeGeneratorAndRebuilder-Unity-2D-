using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Класс значений для настроек квадратной безопасной зоны
    /// </summary>
    public class SquareSafeZoneSettings: ISafeZoneSettings
    {
        public SafeZoneMode Mode => SafeZoneMode.Square;
        
        // Радиус безопасной зоны (квадрат)
        public int Radius { get; }
        
        // Минимальное и максимальное значения радиуса безопасной зоны
        public const int MinRadius = 1;
        public const int MaxRadius = 10;
        
        // Радиус безопасной зоны (квадрат) по умолчанию
        public const int DefaultRadius = 2;
        
        // Коэффициент для определения радиуса безопасной зоны
        public const float RadiusFactor = 0.2f;

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
                throw new ArgumentOutOfRangeException(nameof(radius),
                    $"Expected {MinRadius} <= Radius <= {maxAllowedRadius} " +
                    "(safe zone must fit inside the maze and not occupy the entire area)");
            
            Radius = radius;
        }
        
    }
}