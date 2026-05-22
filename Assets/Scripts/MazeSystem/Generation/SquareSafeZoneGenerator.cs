using System;
using MazeSystem.Core;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Генератор квадратной безопасной зоны
    /// </summary>
    public class SquareSafeZoneGenerator : ISafeZoneGenerator
    {
        /// <summary>
        /// Формирует квадратную безопасную зону
        /// </summary>
        /// <param name="context">Контекст формирования безопасной зоны</param>
        /// <param name="settingsSafeZone">Настройки безопасной зоны</param>
        /// <param name="listener">Слушатель событий безопасной зоны</param>
        /// <returns>Сформированная безопасная зона</returns>
        /// <exception cref="ArgumentException">
        /// Передан неподходящий тип настроек безопасной зоны
        /// </exception>
        public SafeZone Generate(
            SafeZoneContext context, 
            ISafeZoneSettings settingsSafeZone,
            ISafeZoneListener listener = null
            )
        {
            if (settingsSafeZone is not SquareSafeZoneSettings settings)
            {
                throw new ArgumentException(
                    "Wrong settings type for DynamicSafeZoneGenerator");
            }
            
            var maze = context.Maze;
            var zone = new SafeZone(maze.Rows, maze.Cols);
            
            var playerPos = context.PlayerPosition;

            var row = playerPos.x;
            var col = playerPos.y;
            var radius = settings.Radius;
            
            for (int i = row - radius; i <= row + radius; i++)
            {
                for (int j = col - radius; j <= col + radius; j++)
                {
                    if (i >= 0 && i < maze.Rows &&
                        j >= 0 && j < maze.Cols)
                    {
                        zone.SetSafe(i, j, true);
                        if (i == row && j == col)
                        {
                            listener?.OnFloorRepaint(
                                maze.GetCell(i, j), 
                                Color.brown);
                        }
                        else
                        {
                            listener?.OnFloorRepaint(
                                maze.GetCell(i, j), 
                                Color.lightCoral);
                        }
                    }
                }
            }
            
            return zone;
        }
    }
}