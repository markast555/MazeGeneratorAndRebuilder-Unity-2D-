using System;

namespace MazeSystem.Unity.Debugging
{
    /// <summary>
    /// Категории отладки
    /// </summary>
    [Flags]
    public enum DebugCategory
    {
        None = 0,
        Generation = 1,
        SafeZone = 2,
        Create = 4,
        Neighbors = 8,
        Rebuild = 16,
        Player = 32,
        Screenshot = 64,
        All = ~None
    }
}