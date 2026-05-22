using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MazeSystem.Core;
using UnityEngine;
using Random = System.Random;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Генератор лабиринта по алгоритму Growing Tree
    /// </summary>
    public class GrowingTreeGenerator : IContinuableMazeGenerator 
    {
        private readonly Random _random = new();

        /// <summary>
        /// Подготавливает данные для генерации лабиринта
        /// и запускает метод формирования лабиринта
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="listener">Слушатель событий генерации</param>
        public void Generate(Maze maze, IMazeGeneratorListener listener = null)
        {
            var active = new List<Cell>();
            var visited = new HashSet<Cell>();

            var start = maze.GetCell(
                _random.Next(maze.Rows),
                _random.Next(maze.Cols)
            );
            
            visited.Add(start);
            active.Add(start);
            
            ContinueGenerate(
                active,
                visited,
                listener);
            
        }

        /// <summary>
        /// Формирует структуру лабиринта
        /// </summary>
        /// <param name="active">Активные ячейки</param>
        /// <param name="visited">Посещённые ячейки</param>
        /// <param name="listener">Слушатель событий генерации</param>
        public void ContinueGenerate(
            List<Cell> active,
            HashSet<Cell> visited,
            IMazeGeneratorListener listener = null)
        {
            while (active.Count > 0)
            {
                var cell = _random.NextDouble() < 0.85 ? 
                    active[^1] : active[_random.Next(active.Count)];
                
                var neighbors = cell.Neighbors
                    .Where(n => !visited.Contains(n.Value))
                    .ToList();

                if (neighbors.Count > 0)
                {
                    var pair = neighbors[_random.Next(neighbors.Count)];

                    var side = pair.Key;
                    var next = pair.Value;
                    
                    // Удаление стены в логической модели
                    cell.RemoveWall(side);
                    next.RemoveWall(side.GetOpposite());
                    
                    // Уведомление об удалении стены
                    listener?.OnWallRemoved(cell, side);

                    visited.Add(next);
                    active.Add(next);
                }
                else
                {
                    active.Remove(cell);
                    listener?.OnFloorRepaint(cell, Color.white);
                }
            }
        }
        
        /// <summary>
        /// Подготавливает данные для генерации лабиринта
        /// и запускает метод пошагового формирования лабиринта 
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="listener">Слушатель событий генерации</param>
        /// <param name="delay">Время задержки между шагами</param>
        /// <returns>Enumerator для пошаговой генерации</returns>
        public IEnumerator GenerateAnimated(
            Maze maze,
            IMazeGeneratorListener listener,
            float delay)
        {
            var active = new List<Cell>();
            var visited = new HashSet<Cell>();

            var start = maze.GetCell(
                _random.Next(maze.Rows),
                _random.Next(maze.Cols)
            );

            visited.Add(start);
            active.Add(start);

            yield return ContinueGenerateAnimated(
                active,
                visited,
                listener,
                delay);
        }
        
        /// <summary>
        /// Формирует структуру лабиринта пошагово
        /// </summary>
        /// <param name="active">Активные ячейки</param>
        /// <param name="visited">Посещённые ячейки</param>
        /// <param name="listener">Слушатель событий генерации</param>
        /// <param name="delay">Время задержки между шагами</param>
        /// <returns>Enumerator для пошаговой генерации</returns>
        public IEnumerator ContinueGenerateAnimated(
            List<Cell> active,
            HashSet<Cell> visited,
            IMazeGeneratorListener listener,
            float delay)
        {
            while (active.Count > 0)
            {
                var cell = _random.NextDouble() < 0.85 ? active[^1] : active[_random.Next(active.Count)];

                listener?.OnFloorRepaint(cell, Color.chartreuse);

                var neighbors = cell.Neighbors
                    .Where(n => !visited.Contains(n.Value))
                    .ToList();

                if (neighbors.Count > 0)
                {
                    var pair = neighbors[_random.Next(neighbors.Count)];

                    var side = pair.Key;
                    var next = pair.Value;

                    listener?.OnFloorRepaint(
                        next,
                        Color.cornflowerBlue);

                    // Удаление стены в логической модели
                    cell.RemoveWall(side);
                    next.RemoveWall(side.GetOpposite());
                    
                    // Уведомление об удалении стены
                    listener?.OnWallRemoved(cell, side);

                    visited.Add(next);
                    active.Add(next);

                    yield return new WaitForSeconds(delay);

                    listener?.OnFloorRepaint(
                        cell,
                        Color.burlywood);

                    listener?.OnFloorRepaint(
                        next,
                        Color.burlywood);
                }
                else
                {
                    active.Remove(cell);

                    listener?.OnFloorRepaint(
                        cell,
                        Color.white);

                    yield return new WaitForSeconds(delay);
                }
            }
        }
        
    }
}