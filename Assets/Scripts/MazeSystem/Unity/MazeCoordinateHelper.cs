using UnityEngine;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Предоставляет методы для работы с координатами лабиринта
    /// </summary>
    public class MazeCoordinateHelper
    {
        private readonly Transform _transform;
        private readonly float _cellSize;

        /// <summary>
        /// Создаёт помощник для работы с координатами лабиринта
        /// </summary>
        /// <param name="transform">Transform лабиринта</param>
        /// <param name="cellSize">Размер ячейки лабиринта</param>
        public MazeCoordinateHelper(Transform transform, float cellSize)
        {
            _transform = transform;
            _cellSize = cellSize;
        }

        /// <summary>
        /// Переводит мировую позицию в ячейку лабиринта
        /// </summary>
        /// <param name="worldPos">Позиция в мире</param>
        /// <param name="maze">Лабиринт</param>
        /// <param name="cell">Найденная ячейка лабиринта</param>
        /// <returns>
        /// true, если позиция находится внутри лабиринта;
        /// иначе false
        /// </returns>
        public bool TryGetCell(Vector3 worldPos, Maze maze, out Cell cell)
        {
            Vector3 localPos = worldPos - _transform.position;

            int col = Mathf.FloorToInt(localPos.x / _cellSize);
            int row = Mathf.FloorToInt(localPos.y / _cellSize);

            if (row >= 0 && row < maze.Rows && col >= 0 && col < maze.Cols)
            {
                cell = maze.GetCell(row, col);
                return true;
            }

            cell = null;
            return false;
        }

        /// <summary>
        /// Возвращает мировую позицию центра ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <returns>Мировая позиция центра ячейки</returns>
        public Vector3 GetCellCenter(Cell cell)
        {
            return _transform.position + new Vector3(
                cell.Col * _cellSize + _cellSize / 2f,
                cell.Row * _cellSize + _cellSize / 2f,
                0
            );
        }
    }
}