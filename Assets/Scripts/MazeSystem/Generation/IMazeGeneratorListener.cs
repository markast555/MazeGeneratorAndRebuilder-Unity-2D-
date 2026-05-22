using MazeSystem.Core;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Определяет обработку событий генерации лабиринта
    /// </summary>
    public interface IMazeGeneratorListener
    {
        /// <summary>
        /// Вызывается при создании стены
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона</param>
        void OnWallCreated(Cell cell, BorderSide side);
        
        /// <summary>
        /// Вызывается при удалении стены
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона</param>
        void OnWallRemoved(Cell cell, BorderSide side);

        /// <summary>
        /// Вызывается при изменении цвета ячейки (пола)
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="color">Цвет</param>
        void OnFloorRepaint(Cell cell, Color color);
    }
}