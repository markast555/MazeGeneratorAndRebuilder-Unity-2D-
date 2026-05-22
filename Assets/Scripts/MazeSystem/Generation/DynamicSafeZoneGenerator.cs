using System;
using System.Collections.Generic;
using MazeSystem.Core;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Генератор динамической безопасной зоны
    /// </summary>
    public class DynamicSafeZoneGenerator : ISafeZoneGenerator
    {
        /// <summary>
        /// Формирует динамическую безопасную зону
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
            ISafeZoneListener listener = null)
        {
            if (settingsSafeZone is not DynamicSafeZoneSettings settings)
            {
                throw new ArgumentException(
                    "Wrong settings type for DynamicSafeZoneGenerator");
            }

            var maze = context.Maze;
            var zone = new SafeZone(maze.Rows, maze.Cols);

            var playerPos = context.PlayerPosition;

            int startRow = playerPos.x;
            int startCol = playerPos.y;

            int maxDistance = settings.Distance;

            var start = maze.GetCell(startRow, startCol);
            
            // Обход соседних ячеек алгоритмом BFS
            // в пределах максимальной дистанции
            var queue = new Queue<(Cell cell, int dist)>();
            var visited = new HashSet<Cell>();

            queue.Enqueue((start, 0));
            visited.Add(start);

            while (queue.Count > 0)
            {
                var (cell, dist) = queue.Dequeue();

                if (dist > maxDistance)
                    continue;

                PaintCell(zone, listener, cell, dist == 0);

                foreach (var kv in cell.Neighbors)
                {
                    var direction = kv.Key;
                    var neighbor = kv.Value;

                    if (neighbor == null)
                        continue;

                    if (visited.Contains(neighbor))
                        continue;

                    // Нельзя проходить через стены
                    if (cell.HasWall(direction))
                        continue;

                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, dist + 1));
                }
            }

            // Продление безопасной зоны во все стороны
            // от стартовой ячейки при отсутствии стен
            foreach (var kv in start.Neighbors)
            {
                var direction = kv.Key;
                var neighbor = kv.Value;

                if (neighbor == null)
                    continue;

                // Если сразу стена — пропуск направления
                if (start.HasWall(direction))
                    continue;

                // Проход строго по одному направлению
                ExtendDirection(
                    start, direction,
                    zone, listener,
                    visited);
            }

            return zone;
        }

        /// <summary>
        /// Продлевает безопасную зону строго в одном направлении
        /// до первой стены
        /// </summary>
        /// <param name="start">Стартовая ячейка</param>
        /// <param name="direction">Направление расширения</param>
        /// <param name="zone">Безопасная зона</param>
        /// <param name="listener">Слушатель событий безопасной зоны</param>
        /// <param name="visited">Посещённые ячейки</param>
        private void ExtendDirection(
            Cell start,
            BorderSide direction,
            SafeZone zone,
            ISafeZoneListener listener,
            HashSet<Cell> visited)
        {
            Cell current = start;

            while (true)
            {
                // Если впереди стена — конец
                if (current.HasWall(direction))
                    break;

                if (!current.Neighbors.TryGetValue(direction, out var next))
                    break;

                if (next == null)
                    break;

                current = next;
                
                visited.Add(current);

                PaintCell(
                    zone, listener,
                    current, false);


                // Добавление соседних боковых ячеек
                foreach (var kv in current.Neighbors)
                {
                    var side = kv.Key;
                    var sideNeighbor = kv.Value;

                    // Не назад и не вперёд
                    if (side == direction)
                        continue;

                    if (side == direction.GetOpposite())
                        continue;

                    if (sideNeighbor == null)
                        continue;

                    // Если есть стена — пропуск
                    if (current.HasWall(side))
                        continue;

                    if (visited.Contains(sideNeighbor))
                        continue;

                    visited.Add(sideNeighbor);

                    PaintCell(
                        zone, listener,
                        sideNeighbor, false);
                }
            }
        }

        /// <summary>
        /// Добавляет ячейку в безопасную зону и перекрашивает её
        /// </summary>
        /// <param name="zone">Безопасная зона</param>
        /// <param name="listener">Слушатель событий безопасной зоны</param>
        /// <param name="cell">Ячейка</param>
        /// <param name="isCenter">
        /// Является ли ячейка центром безопасной зоны
        /// </param>
        private void PaintCell(
            SafeZone zone,
            ISafeZoneListener listener,
            Cell cell,
            bool isCenter)
        {
            zone.SetSafe(cell.Row, cell.Col, true);

            listener?.OnFloorRepaint(
                cell,
                isCenter ? Color.brown : Color.lightCoral);
        }
    }
}