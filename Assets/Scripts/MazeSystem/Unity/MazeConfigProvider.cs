using System;
using UnityEngine;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Unity-компонент, который хранит и предоставляет
    /// конфигурацию параметров генерации.
    /// Значения задаются через Inspector и преобразуются в runtime-конфиг.
    /// </summary>
    public class MazeConfigProvider : MonoBehaviour
    {
        // Размер лабиринта
        public int mazeRows = MazeSettings.DefaultMazeRows;
        public int mazeCols = MazeSettings.DefaultMazeCols;
        
        // === SafeZone ===
        // Режим определения безопасной зоны
        public SafeZoneMode safeZoneMode = SafeZoneMode.Square;
        
        // Радиус безопасной зоны (квадрат)
        public int squareSafeZoneRadius = SquareSafeZoneSettings.DefaultRadius;
        
        // Расстояние до конца безопасной зоны в ячейках (клетках)
        public int dynamicSafeZoneDistance = DynamicSafeZoneSettings.DefaultDistance;

        /// <summary>
        /// Создаёт объект настроек лабиринта на основе значений из Inspector
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="MazeSettings"/> с валидированными параметрами
        /// </returns>
        public MazeSettings GetMazeSettings()
        {
            return new MazeSettings(
                mazeRows,
                mazeCols
            );
        }

        /// <summary>
        /// Создаёт объект настроек безопасной зоны на основе значений из Inspector 
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="ISafeZoneSettings"/> с валидированными параметрами
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый режим формирования безопасной зоны
        /// </exception>
        public ISafeZoneSettings GetSafeZoneSettings()
        {
            switch (safeZoneMode)
            {
                case SafeZoneMode.Square:
                    return new SquareSafeZoneSettings(
                        squareSafeZoneRadius, mazeRows, mazeCols);

                case SafeZoneMode.Dynamic:
                    return new DynamicSafeZoneSettings(
                        dynamicSafeZoneDistance, mazeRows, mazeCols);

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(safeZoneMode), 
                        safeZoneMode, 
                        "Unsupported SafeZoneMode value");
            }
        }
        
        /// <summary>
        /// Создаёт объект настроек лабиринта и безопасной зоны
        /// на основе значений из Inspector
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="MazeConfig"/> с валидированными параметрами
        /// </returns>
        public MazeConfig GetMazeConfig()
        {
            return new MazeConfig(
                GetMazeSettings(),
                GetSafeZoneSettings());
        }
        
        /// <summary>
        /// Проверяет корректность значений, выставленных в Inspector
        /// </summary>
        public void OnValidate()
        {
            mazeRows = Mathf.Clamp(
                mazeRows, 
                MazeSettings.MinMazeRows, 
                MazeSettings.MaxMazeRows);
            
            mazeCols = Mathf.Clamp(
                mazeCols,
                MazeSettings.MinMazeCols, 
                MazeSettings.MaxMazeCols);
            
            int maxAllowedRadius = SafeZoneCalculator.CalculateMax(
                mazeRows,
                mazeCols,
                SquareSafeZoneSettings.MinRadius,
                SquareSafeZoneSettings.MaxRadius,
                SquareSafeZoneSettings.RadiusFactor
            );
            
            squareSafeZoneRadius = Mathf.Clamp(
                squareSafeZoneRadius,
                SquareSafeZoneSettings.MinRadius,
                maxAllowedRadius);
            
            int maxAllowedDistance = SafeZoneCalculator.CalculateMax(
                mazeRows,
                mazeCols,
                DynamicSafeZoneSettings.MinDistance,
                DynamicSafeZoneSettings.MaxDistance,
                DynamicSafeZoneSettings.DistanceFactor
            );
            
            dynamicSafeZoneDistance = Mathf.Clamp(
                dynamicSafeZoneDistance,
                DynamicSafeZoneSettings.MinDistance,
                maxAllowedDistance);
        }
        
        /// <summary>
        /// Ставит значения по умолчанию в Inspector 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый режим формирования безопасной зоны
        /// </exception>
        public void ResetToDefault()
        {
            mazeRows = MazeSettings.DefaultMazeRows;
            mazeCols = MazeSettings.DefaultMazeCols;

            switch (safeZoneMode)
            {
                case SafeZoneMode.Square:
                    squareSafeZoneRadius = SquareSafeZoneSettings.DefaultRadius;
                    break;

                case SafeZoneMode.Dynamic:
                    dynamicSafeZoneDistance = DynamicSafeZoneSettings.DefaultDistance;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(safeZoneMode), 
                        safeZoneMode, 
                        "Unsupported SafeZoneMode value");
            }
            
        }
    }
}