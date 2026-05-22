using UnityEngine;

namespace MazeSystem.Unity.Debugging
{
    /// <summary>
    /// Логирование сообщений отладки в консоль Unity
    /// </summary>
    public static class DebugLogger
    {
        /// <summary>
        /// Выводит сообщение в консоль с указанием категории
        /// </summary>
        /// <param name="message">Сообщение для вывода</param>
        /// <param name="category">Категория отладки</param>
        public static void Log(string message, DebugCategory category)
        {
            if (DebugContext.Config != null &&
                DebugContext.Config.IsEnabled(category))
            {
                Debug.Log($"[{category}] {message}");
            }
        }
    }
}