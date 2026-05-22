namespace MazeSystem.Core
{
    /// <summary>
    /// Конфигурация лабиринта и безопасной зоны
    /// </summary>
    public class MazeConfig
    {
        public MazeSettings Maze { get; }
        public ISafeZoneSettings SafeZone { get; }

        public MazeConfig(MazeSettings maze, ISafeZoneSettings safeZone)
        {
            Maze = maze;
            SafeZone = safeZone;
        }
    }
}