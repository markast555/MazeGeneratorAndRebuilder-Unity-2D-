using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Источник позиции на основе Transform
    /// </summary>
    public class TransformPositionProvider : MonoBehaviour, IPositionProvider
    {
        /// <summary>
        /// Возвращает мировую позицию объекта,
        /// к которому прикреплён компонент
        /// </summary>
        /// <returns>Мировая позиция</returns>
        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }
    }
}