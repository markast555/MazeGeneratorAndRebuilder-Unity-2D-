using System.Collections;
using System.Collections.Generic;
using MazeSystem.Core;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Определяет функциональность генератора лабиринта с продолжением
    /// </summary>
    public interface IContinuableMazeGenerator : IMazeGenerator
    {
        /// <summary>
        /// Формирует структуру лабиринта
        /// </summary>
        /// <param name="active">Активные ячейки</param>
        /// <param name="visited">Посещённые ячейки</param>
        /// <param name="listener">Слушатель событий генерации</param>
        void ContinueGenerate(
            List<Cell> active,
            HashSet<Cell> visited,
            IMazeGeneratorListener listener = null);

        /// <summary>
        /// Формирует структуру лабиринта пошагово
        /// </summary>
        /// <param name="active">Активные ячейки</param>
        /// <param name="visited">Посещённые ячейки</param>
        /// <param name="delay">Время задержки между шагами</param>
        /// <returns>Enumerator для пошаговой генерации</returns>
        IEnumerator ContinueGenerateAnimated(
            List<Cell> active,
            HashSet<Cell> visited,
            IMazeGeneratorListener listener,
            float delay);
    }
}