using System.Collections.Generic;

namespace MazeSystem.Core
{
    /// <summary>
    /// Ячейка лабиринта
    /// </summary>
    public class Cell
    {
        public int Row { get; }
        public int Col { get; }

        /// <summary>
        /// Стены текущей ячейки
        /// </summary>
        public BorderSide Walls { get; private set; } = BorderSide.None;

        /// <summary>
        /// Соседние ячейки по сторонам
        /// </summary>
        public Dictionary<BorderSide, Cell> Neighbors { get; } = new();

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        /// <summary>
        /// Проверяет, есть ли стена с указанной стороны
        /// </summary>
        /// <param name="side">Сторона ячейки</param>
        /// <returns>True, если стена присутствует, иначе false</returns>
        public bool HasWall(BorderSide side)
        {
            return Walls.HasFlag(side);
        }

        /// <summary>
        /// Добавляет стену с указанной стороны
        /// </summary>
        /// <param name="side">Сторона ячейки</param>
        public void AddWall(BorderSide side)
        {
            Walls |= side;
        }

        /// <summary>
        /// Удаляет стену с указанной стороны
        /// </summary>
        /// <param name="side">Сторона ячейки</param>
        public void RemoveWall(BorderSide side)
        {
            Walls &= ~side;
        }
    }
}