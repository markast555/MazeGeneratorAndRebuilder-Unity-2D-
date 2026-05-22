using System;

namespace MazeSystem.Core
{
    /// <summary>
    /// Методы-расширения для BorderSide
    /// </summary>
    public static class BorderSideExtensions
    {
        /// <summary>
        /// Возвращает противоположную сторону
        /// </summary>
        /// <param name="side">Сторона ячейки</param>
        /// <returns>Сторона ячейки</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый тип стороны
        /// </exception>
        public static BorderSide GetOpposite(this BorderSide side)
        {
            return side switch
            {
                BorderSide.Top => BorderSide.Bottom,
                BorderSide.Bottom => BorderSide.Top,
                BorderSide.Left => BorderSide.Right,
                BorderSide.Right => BorderSide.Left,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(side), side, "Unsupported BorderSide value")
            };
        }
    }
}