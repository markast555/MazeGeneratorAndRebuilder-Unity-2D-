using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Класс по работе с BorderSide.
    /// </summary>
    public static class BorderSideExtensions
    {
        /// <summary>
        /// Получает противоположную сторону указанной границы.
        /// </summary>
        /// <param name="side">Сторона ячейки</param>
        /// <returns>Сторона ячейки</returns>
        public static BorderSide GetOpposite(this BorderSide side)
        {
            return side switch
            {
                BorderSide.Top => BorderSide.Bottom,
                BorderSide.Bottom => BorderSide.Top,
                BorderSide.Left => BorderSide.Right,
                BorderSide.Right => BorderSide.Left,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(side), side, "Invalid border side")
            };
        }
    }
}