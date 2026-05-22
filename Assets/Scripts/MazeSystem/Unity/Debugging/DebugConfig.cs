using UnityEngine;

namespace MazeSystem.Unity.Debugging
{
    /// <summary>
    /// Конфигурация отладки
    /// </summary>
    public class DebugConfig : MonoBehaviour
    {
        [SerializeField] private bool debugMode = true;
    
        [SerializeField]
        private DebugCategory categories = DebugCategory.All;

        /// <summary>
        /// Проверяет, включена ли указанная категория отладки
        /// </summary>
        /// <param name="category">Категория отладки</param>
        /// <returns>Истина, если указанная категория отладки включена</returns>
        public bool IsEnabled(DebugCategory category)
        {
            return debugMode && (categories & category) != 0;
        }
    }
}