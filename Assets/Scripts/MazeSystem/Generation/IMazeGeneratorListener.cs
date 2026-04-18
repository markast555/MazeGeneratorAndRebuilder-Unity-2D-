using MazeSystem.Core;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Интерфейс для отслеживания шагов генерации
    /// </summary>
    public interface IMazeGeneratorListener
    {
        /// <summary>
        /// Удаляет стену с указанной стороны ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона</param>
        void OnWallRemoved(Cell cell, BorderSide side);

        /// <summary>
        /// Перекрашивает ячейку (пол) в указанный цвет
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="color">Цвет</param>
        void OnFloorRepaint(Cell cell, Color color);
    }
}