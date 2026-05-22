using MazeSystem.Core;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Определяет обработку событий генерации безопасной зоны
    /// </summary>
    public interface ISafeZoneListener
    {
        /// <summary>
        /// Вызывается при изменении цвета ячейки (пола)
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="color">Цвет</param>
        void OnFloorRepaint(Cell cell, Color color);
    }
}