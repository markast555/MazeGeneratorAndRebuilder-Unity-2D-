using System;

namespace MazeSystem.Core
{
    
    /// <summary>
    /// Вспомогательные вычисления, связанные с параметрами безопасной зоны
    /// </summary>
    public static class SafeZoneCalculator
    {
        /// <summary>
        /// Вычисляет максимально допустимый размер параметра безопасной зоны
        /// на основе размеров лабиринта и коэффициента масштабирования
        /// </summary>
        /// <param name="mazeRows">Количество строк в лабиринте</param>
        /// <param name="mazeCols">Количество столбцов в лабиринте</param>
        /// <param name="min">Минимальное допустимое значение</param>
        /// <param name="max">Максимальное допустимое значение</param>
        /// <param name="factor">Коэффициент масштабирования безопасной зоны</param>
        /// <returns>Максимально допустимое значение параметра безопасной зоны</returns>
        public static int CalculateMax(
            int mazeRows,
            int mazeCols,
            int min,
            int max,
            float factor)
        {
            return Math.Min(
                max,
                Math.Max(
                    min,
                    (int)(Math.Min(mazeRows, mazeCols) * factor)
                )
            );
        }
    }
}