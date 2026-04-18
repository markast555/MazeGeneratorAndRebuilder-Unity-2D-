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
    public class GrowingTreeGenerator : IMazeGenerator
    {
        private readonly Random _random = new();

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

            while (active.Count > 0)
            {
                Cell cell;

                if (_random.NextDouble() < 0.85)
                    cell = active[^1];
                else
                    cell = active[_random.Next(active.Count)];
                
                var neighbors = cell.Neighbors
                    .Where(n => !visited.Contains(n.Value))
                    .ToList();

                if (neighbors.Count > 0)
                {
                    var pair = neighbors[_random.Next(neighbors.Count)];

                    var side = pair.Key;
                    var next = pair.Value;
    
                    // изменение модели
                    cell.RemoveWall(side);
                    next.RemoveWall(side.GetOpposite());

                    // уведомление визуализации
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
        
        public IEnumerator GenerateAnimated(Maze maze, IMazeGeneratorListener listener, float delay)
        {
            var active = new List<Cell>();
            var visited = new HashSet<Cell>();

            var start = maze.GetCell(
                _random.Next(maze.Rows),
                _random.Next(maze.Cols)
            );

            visited.Add(start);
            active.Add(start);

            while (active.Count > 0)
            {
                Cell cell;

                if (_random.NextDouble() < 0.85)
                    cell = active[^1];
                else
                    cell = active[_random.Next(active.Count)];

                listener?.OnFloorRepaint(cell, Color.chartreuse);

                var neighbors = cell.Neighbors
                    .Where(n => !visited.Contains(n.Value))
                    .ToList();

                if (neighbors.Count > 0)
                {
                    var pair = neighbors[_random.Next(neighbors.Count)];

                    var side = pair.Key;
                    var next = pair.Value;

                    listener?.OnFloorRepaint(next, Color.cornflowerBlue);

                    // модель
                    cell.RemoveWall(side);
                    // Debug.Log("First " + side);
                    next.RemoveWall(side.GetOpposite());
                    // Debug.Log("Second " + side);
                    // визуализация
                    listener?.OnWallRemoved(cell, side);

                    visited.Add(next);
                    active.Add(next);

                    yield return new WaitForSeconds(delay); // 🔥 ВАЖНО
                    
                    listener?.OnFloorRepaint(cell, Color.burlywood);
                    listener?.OnFloorRepaint(next, Color.burlywood);

                }
                else
                {
                    active.Remove(cell);
                    listener?.OnFloorRepaint(cell, Color.white);

                    yield return new WaitForSeconds(delay); // 🔥 ВАЖНО
                }
            }
        }
    }
}