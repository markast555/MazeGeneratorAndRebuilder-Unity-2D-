using System;
using UnityEngine;
using MazeSystem.Core;
using UnityEngine.Serialization;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Unity-компонент, который хранит и предоставляет
    /// конфигурацию параметров генерации.
    /// Значения задаются через Inspector и преобразуются в runtime-конфиг.
    /// </summary>
    public class MazeConfigProvider : MonoBehaviour
    {
        // Размер сетки
        // public int tilemapRows = MazeSettings.DefaultTilemapRows;
        // public int tilemapCols = MazeSettings.DefaultTilemapCols;
        //
        // // Начальная позиция лабиринта на сетке
        // public int mazeStartRow = MazeSettings.DefaultMazeStartRow;
        // public int mazeStartCol = MazeSettings.DefaultMazeStartCol;

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
        /// Создаёт объект настроек лабиринта на основе значений из Inspector.
        /// </summary>
        /// <returns>Экземпляр <see cref="MazeSettings"/> с валидированными параметрами.</returns>
        public MazeSettings GetMazeSettings()
        {
            return new MazeSettings(
                // tilemapRows,
                // tilemapCols,
                mazeRows,
                mazeCols
                // mazeStartRow,
                // mazeStartCol,
                // safeZoneSquareRadius
            );
        }

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
                    throw new ArgumentOutOfRangeException();
            }
        }
        
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
            // tilemapRows = Mathf.Clamp(
            //     tilemapRows, MazeSettings.MinTilemapRows, 
            //     MazeSettings.MaxTilemapRows);
            //
            // tilemapCols = Mathf.Clamp(
            //     tilemapCols, 
            //     MazeSettings.MinTilemapCols, 
            //     MazeSettings.MaxTilemapCols);
            //
            // mazeStartRow = Mathf.Clamp(
            //     mazeStartRow,
            //     MazeSettings.MinMazeStartRow,
            //     tilemapRows -  MazeSettings.MinMazeRows);
            //
            // mazeStartCol = Mathf.Clamp(
            //     mazeStartCol,
            //     MazeSettings.MinMazeStartCol,
            //     tilemapCols -  MazeSettings.MinMazeCols);
            
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
        
        public void ResetToDefault()
        {
            // tilemapRows = MazeSettings.DefaultTilemapRows;
            // tilemapCols = MazeSettings.DefaultTilemapCols;
            //
            // mazeStartRow = MazeSettings.DefaultMazeStartRow;
            // mazeStartCol = MazeSettings.DefaultMazeStartCol;

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
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}