using System.Collections.Generic;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Хранит соответствие логических ячеек
    /// их визуальным представлениям
    /// </summary>
    public class MazeViewMap
    {
        private Dictionary<Cell, CellViewData> _cellViews = new();

        /// <summary>
        /// Связывает логическую ячейку
        /// с её визуальным представлением
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void Add(Cell cell, CellViewData viewData)
        {
            _cellViews[cell] = viewData;
        }

        /// <summary>
        /// Возвращает визуальное представление ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <returns>Визуальное представление ячейки</returns>
        public CellViewData Get(Cell cell)
        {
            return _cellViews[cell];
        }

        /// <summary>
        /// Очищает данные визуальных представлений
        /// </summary>
        public void Clear()
        {
            _cellViews.Clear();
        }
    }
}