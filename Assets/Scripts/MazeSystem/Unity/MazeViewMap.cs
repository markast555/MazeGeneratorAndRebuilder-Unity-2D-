using System.Collections.Generic;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Класс-словарь для сопоставления
    /// логической ячейки с её визуальным представлением
    /// </summary>
    public class MazeViewMap
    {
        private Dictionary<Cell, CellViewData> _view = new();

        /// <summary>
        /// Связывает логической ячейки с её визуальным представлением
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="viewData">Визуальное представление ячейки</param>
        public void Add(Cell cell, CellViewData viewData)
        {
            _view[cell] = viewData;
        }

        /// <summary>
        /// Получает визуальные данные ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <returns>Визуальное представление ячейки</returns>
        public CellViewData Get(Cell cell)
        {
            return _view[cell];
        }

        /// <summary>
        /// Очищает словарь
        /// </summary>
        public void Clear()
        {
            _view.Clear();
        }
    }
}