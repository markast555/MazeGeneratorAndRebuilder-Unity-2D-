using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Настройки динамической безопасной зоны
    /// </summary>
    public class DynamicSafeZoneSettings: ISafeZoneSettings
    {
        public SafeZoneMode Mode => SafeZoneMode.Dynamic;
        
        /// <summary>
        /// Расстояние до конца безопасной зоны в ячейках (клетках)
        /// </summary>
        public int Distance { get; }
        
        // Минимальное и максимальное значения расстояния безопасной зоны
        public const int MinDistance = 3;
        public const int MaxDistance = 10;
        
        // Расстояние до конца безопасной зоны по умолчанию
        public const int DefaultDistance = 4;
        
        // Коэффициент масштабирования безопасной зоны
        public const float DistanceFactor = 0.4f;
        
        /// <summary>
        /// Создаёт настройки квадратной безопасной зоны
        /// </summary>
        /// <param name="distance">Расстояние до конца безопасной</param>
        /// <param name="mazeRows">Количество строк в лабиринте</param>
        /// <param name="mazeCols">Количество столбцов в лабиринте</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрано значение расстояния, которое не помещается в размеры лабиринта
        /// или выходит за допустимые границы
        /// </exception>
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
                throw new ArgumentOutOfRangeException(nameof(distance), distance,
                    $"Expected {MinDistance} <= Distance <= {maxAllowedDistance} " +
                    "(safe zone must fit inside the maze and not occupy the entire area)");
            
            Distance = distance;
        }
        
    }
}