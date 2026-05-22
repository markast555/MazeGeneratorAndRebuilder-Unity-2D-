using System.Collections;
using System.Collections.Generic;
using MazeSystem.Core;
using MazeSystem.Unity.Debugging;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Перестройщик лабиринта
    /// </summary>
    public class MazeRebuilder
    {
        /// <summary>
        /// Перестраивает лабиринт, сохраняя безопасную зону
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="safeZone">Безопасная зона</param>
        /// <param name="generator">Генератор лабиринта</param>
        /// <param name="listener">Слушатель событий генерации</param>
        public void Rebuild(
            Maze maze,
            SafeZone safeZone,
            IContinuableMazeGenerator generator,
            IMazeGeneratorListener listener = null)
        {
            var visited = new HashSet<Cell>();
            var active = new List<Cell>();
            
            foreach (var cell in maze.AllCells())
            {
                bool isSafe = safeZone.IsSafe(
                    cell.Row,
                    cell.Col
                );

                // Для сохранения безопасной зоны - ячейки, входящие в неё,
                // отмечаются как посещённые, но не как активные, чтобы они
                // не принимали участие в алгоритме генерации
                if (isSafe)
                {
                    visited.Add(cell);
                    continue;
                }

                listener?.OnFloorRepaint(cell, Color.gray);

                ProcessCellConnections(
                    safeZone,
                    cell,
                    visited,
                    active,
                    listener
                );
            }
            
            generator.ContinueGenerate(
                active,
                visited,
                listener
            );
            
            // === Восстановление связанности лабиринта после перегенерации ===
            FixDisconnectedRegions(
                maze,
                safeZone,
                listener
            );

            RemoveCycles(
                maze,
                safeZone,
                listener
            );
        }
        
        /// <summary>
        /// Перестраивает лабиринт пошагово, сохраняя безопасную зону
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="safeZone">Безопасная зона</param>
        /// <param name="generator">Генератор лабиринта</param>
        /// <param name="listener">Слушатель событий генерации</param>
        /// <param name="delay">Время задержки между шагами</param>
        /// <returns>Enumerator для пошаговой перестройки</returns>
        public IEnumerator RebuildAnimated(
            Maze maze,
            SafeZone safeZone,
            IContinuableMazeGenerator generator,
            IMazeGeneratorListener listener,
            float delay)
        {
            var visited = new HashSet<Cell>();
            var active = new List<Cell>();

            foreach (var cell in maze.AllCells())
            {
                bool isSafe = safeZone.IsSafe(
                    cell.Row,
                    cell.Col
                );
                
                // Для сохранения безопасной зоны - ячейки, входящие в неё,
                // отмечаются как посещённые, но не как активные, чтобы они
                // не принимали участие в алгоритме генерации
                if (isSafe)
                {
                    visited.Add(cell);
                    continue;
                }

                listener?.OnFloorRepaint(cell, Color.gray);

                ProcessCellConnections(
                    safeZone,
                    cell,
                    visited,
                    active,
                    listener
                );
            }

            // Задержка кадра для фиксации подготовки лабиринта к перегенерации
            if (DebugContext.Config != null &&
                DebugContext.Config.IsEnabled(DebugCategory.Screenshot))
            {
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot("DebugScreens/screen_2_rebuild_grid.png");
                yield return null;
            }

            yield return generator.ContinueGenerateAnimated(
                active,
                visited,
                listener,
                delay
            );
            
            // Задержка кадра для фиксации лабиринта после перегенерации
            if (DebugContext.Config != null &&
                DebugContext.Config.IsEnabled(DebugCategory.Screenshot))
            {
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot("DebugScreens/screen_3_after.png");
                yield return null;
            }

            // === Восстановление связанности лабиринта после перегенерации ===
            FixDisconnectedRegions(
                maze,
                safeZone,
                listener
            );

            RemoveCycles(
                maze,
                safeZone,
                listener
            );
        }
        
        /// <summary>
        /// Обрабатывает ячейку с учётом соседей для подготовки к перегенерации
        /// </summary>
        /// <param name="safeZone">Безопасная зона</param>
        /// <param name="cell">Ячейка</param>
        /// <param name="visited">Посещённые ячейки</param>
        /// <param name="active">Активные ячейки</param>
        /// <param name="listener">Слушатель событий генерации</param>
        private void ProcessCellConnections(
            SafeZone safeZone,
            Cell cell,
            HashSet<Cell> visited,
            List<Cell> active,
            IMazeGeneratorListener listener)
        {
            bool hasSafeConnection = false;

            foreach (var kv in cell.Neighbors)
            {
                var side = kv.Key;
                var neighbor = kv.Value;

                if (neighbor == null)
                    continue;

                bool neighborSafe = safeZone.IsSafe(neighbor.Row, neighbor.Col);


                if (neighborSafe && !cell.HasWall(side))
                {
                    hasSafeConnection = true;
                    continue;
                }

                // Изолирование ячеек вне безопасной зоны
                // для исключения их участия в генерации Growing Tree
                if (!neighborSafe)
                {
                    if (!cell.HasWall(side))
                    {
                        cell.AddWall(side);
                        neighbor.AddWall(side.GetOpposite());
                        listener?.OnWallCreated(cell, side);
                    }
                }
            }
            
            // Включение текущей ячейки в список активных для алгоритма Growing Tree
            // при наличии доступного соседа из безопасной зоны
            if (hasSafeConnection)
            {
                visited.Add(cell);
                if (!active.Contains(cell))
                    active.Add(cell);
            }
        }
        
        /// <summary>
        /// Соединяет изолированные области лабиринта
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="safeZone">Безопасная зона</param>
        /// <param name="listener">Слушатель событий генерации</param>
        private void FixDisconnectedRegions(
            Maze maze,
            SafeZone safeZone,
            IMazeGeneratorListener listener)
        {
            var visited = new HashSet<Cell>();

            var start = maze.GetCell(0, 0);

            FloodFill(start, visited);

            foreach (var cell in maze.AllCells())
            {
                if (visited.Contains(cell))
                    continue;
                
                if (safeZone.IsSafe(cell.Row, cell.Col))
                    continue;

                foreach (var kv in cell.Neighbors)
                {
                    var side = kv.Key;
                    var neighbor = kv.Value;

                    if (neighbor == null)
                        continue;
                    
                    if (!visited.Contains(neighbor))
                        continue;
                    
                    if (safeZone.IsSafe(neighbor.Row, neighbor.Col))
                        continue;
                    
                    cell.RemoveWall(side);
                    neighbor.RemoveWall(side.GetOpposite());

                    listener?.OnWallRemoved(cell, side);

                    FloodFill(cell, visited);

                    break;
                }
            }
        }
        
        /// <summary>
        /// Выполняет обход связной области графа, помечая достижимые ячейки
        /// </summary>
        /// <param name="start">Стартовая ячейка</param>
        /// <param name="visited">Посещённые ячейки</param>
        private void FloodFill(
            Cell start,
            HashSet<Cell> visited)
        {
            var stack = new Stack<Cell>();

            stack.Push(start);
            visited.Add(start);

            while (stack.Count > 0)
            {
                var cell = stack.Pop();

                foreach (var kv in cell.Neighbors)
                {
                    var side = kv.Key;
                    var neighbor = kv.Value;

                    if (neighbor == null)
                        continue;
                    
                    if (cell.HasWall(side))
                        continue;

                    if (visited.Contains(neighbor))
                        continue;

                    visited.Add(neighbor);
                    stack.Push(neighbor);
                }
            }
        }
        
        /// <summary>
        /// Удаляет циклы в структуре лабиринта,
        /// проверяя сохранение связности после добавления стен
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="safeZone">Безопасная зона</param>
        /// <param name="listener">Слушатель событий генерации</param>
        private void RemoveCycles(
            Maze maze,
            SafeZone safeZone,
            IMazeGeneratorListener listener)
        {
            foreach (var cell in maze.AllCells())
            {
                foreach (var kv in cell.Neighbors)
                {
                    var side = kv.Key;
                    var neighbor = kv.Value;

                    if (neighbor == null)
                        continue;

                    // Top и Right, потому что метод создания стены на сцене
                    // создаёт стены только с этой стороны ячейки
                    if (side != BorderSide.Right &&
                        side != BorderSide.Top)
                        continue;
                    
                    if (cell.HasWall(side))
                        continue;
                    
                    if (safeZone.IsSafe(cell.Row, cell.Col))
                        continue;

                    if (safeZone.IsSafe(neighbor.Row, neighbor.Col))
                        continue;
                    
                    // Предварительное добавление стены в логике
                    cell.AddWall(side);
                    neighbor.AddWall(side.GetOpposite());

                    var visited = new HashSet<Cell>();

                    // Проверка на наличие замкнутых областей
                    FloodFill(cell, visited);

                    bool disconnected = false;

                    foreach (var c in maze.AllCells())
                    {
                        if (safeZone.IsSafe(c.Row, c.Col))
                            continue;

                        if (!visited.Contains(c))
                        {
                            disconnected = true;
                            break;
                        }
                    }
                    
                    // Если добавление стены нарушает связность,
                    // то она удаляется, иначе создаётся и на сцене
                    if (disconnected)
                    {
                        cell.RemoveWall(side);
                        neighbor.RemoveWall(side.GetOpposite());
                    }
                    else
                    {
                        listener?.OnWallCreated(cell, side);
                    }
                }
            }
        }
        
    }
}