using MazeSystem.Core;
using UnityEngine;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Интерфейс для отслеживания шагов генерации
    /// </summary>
    public interface IMazeGeneratorListener
    {
        void OnWallRemoved(Cell cell, BorderSide side);

        void OnFloorRepaint(Cell cell, Color color);
    }
}