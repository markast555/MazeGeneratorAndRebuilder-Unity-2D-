using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Источник мировой позиции объекта
    /// </summary>
    public interface IPositionProvider
    {
        /// <summary>
        /// Возвращает мировую позицию объекта
        /// </summary>
        /// <returns>Позиция в мировых координатах</returns>
        Vector3 GetWorldPosition();
    }
}