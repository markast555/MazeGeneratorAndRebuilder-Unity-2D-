using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Стороны границы ячейки лабиринта
    /// </summary>
    [Flags]
    public enum BorderSide
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        All = Top | Bottom | Left | Right
    }
}