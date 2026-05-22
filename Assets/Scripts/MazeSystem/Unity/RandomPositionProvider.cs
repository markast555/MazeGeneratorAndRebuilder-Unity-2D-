using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Источник случайной позиции
    /// </summary>
    public class RandomPositionProvider : MonoBehaviour, IPositionProvider
    {
        [SerializeField] private float min = -15f;
        [SerializeField] private float max = 15f;

        /// <summary>
        /// Возвращает случайную мировую позицию
        /// </summary>
        /// <returns>Мировая позиция</returns>
        public Vector3 GetWorldPosition()
        {
            return new Vector3(
                Random.Range(min, max),
                Random.Range(min, max),
                0
            );
        }
    }
}