using System.Collections;
using MazeSystem.Core;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Интерфейс генератора лабиринта
    /// </summary>
    public interface IMazeGenerator
    {
        /// <summary>
        /// Генерирует лабиринт
        /// </summary>
        /// <param name="maze">Модель лабиринта</param>
        /// <param name="listener">
        /// Слушатель событий генерации (опционально, для визуализации)
        /// </param>
        void Generate(Maze maze, IMazeGeneratorListener listener = null);
        
        IEnumerator GenerateAnimated(Maze maze, IMazeGeneratorListener listener, float delay);
    }
}