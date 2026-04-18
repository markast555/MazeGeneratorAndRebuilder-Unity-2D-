namespace MazeSystem.Core
{
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