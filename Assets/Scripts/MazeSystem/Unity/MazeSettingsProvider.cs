using System;
using UnityEngine;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Класс-проводник с классом MazeSettings,
    /// задающий значения по умолчанию
    /// с возможностью их изменения в Inspector
    /// </summary>
    public class MazeSettingsProvider : MonoBehaviour
    {
        // Размер сетки
        [Header("Tilemap")]
        [Min(MazeSettings.MinTilemapRows)] 
        public int tilemapRows = MazeSettings.DefaultTilemapRows;
        [Min(MazeSettings.MinTilemapCols)] 
        public int tilemapCols = MazeSettings.DefaultTilemapCols;
        
        // Начальная позиция лабиринта на сетке
        [Header("Maze Start Position")]
        [Min(MazeSettings.MinMazeStartRow)] 
        public int mazeStartRow = MazeSettings.DefaultMazeStartRow;
        [Min(MazeSettings.MinMazeStartCol)] 
        public int mazeStartCol = MazeSettings.DefaultMazeStartCol;

        // Размер лабиринта
        [Header("Maze Size")]
        [Min(MazeSettings.MinMazeRows)] 
        public int mazeRows = MazeSettings.DefaultMazeRows;
        [Min(MazeSettings.MinMazeCols)] 
        public int mazeCols = MazeSettings.DefaultMazeCols;
        
        // Радиус безопасной зоны (квадрат)
        [Header("Safe Zone")]
        [Min(MazeSettings.MinSafeZoneSquareRadius)] 
        public int safeZoneSquareRadius = MazeSettings.DefaultSafeZoneSquareRadius;

        /// <summary>
        /// Создаёт объект настроек лабиринта на основе значений из Inspector.
        /// </summary>
        /// <returns>Экземпляр <see cref="MazeSettings"/> с валидированными параметрами.</returns>
        public MazeSettings CreateSettings()
        {
            return new MazeSettings(
                tilemapRows,
                tilemapCols,
                mazeRows,
                mazeCols,
                mazeStartRow,
                mazeStartCol,
                safeZoneSquareRadius
            );
        }
        
        /// <summary>
        /// Проверяет корректность значений, выставленных в Inspector
        /// </summary>
        public void OnValidate()
        {
            tilemapRows = Mathf.Clamp(
                tilemapRows, MazeSettings.MinTilemapRows, 
                MazeSettings.MaxTilemapRows);
            
            tilemapCols = Mathf.Clamp(
                tilemapCols, 
                MazeSettings.MinTilemapCols, 
                MazeSettings.MaxTilemapCols);
            
            mazeStartRow = Mathf.Clamp(
                mazeStartRow,
                MazeSettings.MinMazeStartRow,
                tilemapRows -  MazeSettings.MinMazeRows);
            
            mazeStartCol = Mathf.Clamp(
                mazeStartCol,
                MazeSettings.MinMazeStartCol,
                tilemapCols -  MazeSettings.MinMazeCols);
            
            mazeRows = Mathf.Clamp(
                mazeRows, 
                MazeSettings.MinMazeRows, 
                tilemapRows - mazeStartRow);
            
            mazeCols = Mathf.Clamp(
                mazeCols,
                MazeSettings.MinMazeCols, 
                tilemapCols - mazeStartCol);
            
            int maxAllowedRadius = Mathf.Min(
                MazeSettings.MaxSafeZoneSquareRadius, 
                Mathf.Max(MazeSettings.MinSafeZoneSquareRadius, 
                    (int)(Mathf.Min(mazeRows, mazeCols) * MazeSettings.SafeZoneFactor)));
            
            safeZoneSquareRadius = Mathf.Clamp(
                safeZoneSquareRadius,
                MazeSettings.MinSafeZoneSquareRadius,
                maxAllowedRadius);
        }
        
        public void ResetToDefault()
        {
            tilemapRows = MazeSettings.DefaultTilemapRows;
            tilemapCols = MazeSettings.DefaultTilemapCols;

            mazeStartRow = MazeSettings.DefaultMazeStartRow;
            mazeStartCol = MazeSettings.DefaultMazeStartCol;

            mazeRows = MazeSettings.DefaultMazeRows;
            mazeCols = MazeSettings.DefaultMazeCols;

            safeZoneSquareRadius = MazeSettings.DefaultSafeZoneSquareRadius;
        }
    }
}