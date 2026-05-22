using System.Collections;
using MazeSystem.Core;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Определяет функциональность генератора лабиринта
    /// </summary>
    public interface IMazeGenerator
    {
        /// <summary>
        /// Формирует структуру лабиринта
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="listener">Слушатель событий генерации</param>
        void Generate(Maze maze, IMazeGeneratorListener listener = null);
        
        /// <summary>
        /// Формирует структуру лабиринта пошагово
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="listener">Слушатель событий генерации</param>
        /// <param name="delay">Время задержки между шагами</param>
        /// <returns>Enumerator для пошаговой генерации</returns>
        IEnumerator GenerateAnimated(Maze maze, IMazeGeneratorListener listener, float delay);
    }
}