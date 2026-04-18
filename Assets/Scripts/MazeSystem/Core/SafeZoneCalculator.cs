using System;

namespace MazeSystem.Core
{
    public static class SafeZoneCalculator
    {
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