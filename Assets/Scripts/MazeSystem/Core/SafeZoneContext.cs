using UnityEngine;

namespace MazeSystem.Core
{
    /// <summary>
    /// Контекст для вычисления безопасной зоны
    /// </summary>
    public class SafeZoneContext
    {
        public Maze Maze { get; set; }
        public Vector2Int PlayerPosition { get; set; }
    }
}