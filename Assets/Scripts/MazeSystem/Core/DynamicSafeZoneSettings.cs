using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Класс значений для настроек вычисляемой безопасной зоны
    /// </summary>
    public class DynamicSafeZoneSettings: ISafeZoneSettings
    {
        public SafeZoneMode Mode => SafeZoneMode.Dynamic;
        
        // Расстояние до конца безопасной зоны в ячейках (клетках)
        public int Distance { get; }
        
        // Минимальное и максимальное значения расстояния безопасной зоны
        public const int MinDistance = 3;
        public const int MaxDistance = 10;
        
        // Расстояние до конца безопасной зоны по умолчанию
        public const int DefaultDistance = 4;
        
        // Коэффициент для определения расстояния до конца безопасной зоны
        public const float DistanceFactor = 0.4f;
        
        public DynamicSafeZoneSettings(
            int distance,
            int mazeRows,
            int mazeCols
            )
        {
            int maxAllowedDistance = SafeZoneCalculator.CalculateMax(
                mazeRows,
                mazeCols,
                MinDistance,
                MaxDistance,
                DistanceFactor
            );
            
            if (distance > maxAllowedDistance || distance < MinDistance)
                throw new ArgumentOutOfRangeException(nameof(distance),
                    $"Expected {MinDistance} <= Distance <= {maxAllowedDistance} " +
                    "(safe zone must fit inside the maze and not occupy the entire area)");
            
            Distance = distance;
        }
        
    }
}