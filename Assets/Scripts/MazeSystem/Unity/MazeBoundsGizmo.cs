using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Отрисовывает границы лабиринта с помощью Gizmos
    /// </summary>
    public class MazeBoundsGizmo : MonoBehaviour
    {
        [SerializeField] private MazeConfigProvider mazeConfigProvider;
        [SerializeField] private MazeRenderer mazeRenderer;
        
        /// <summary>
        /// Рисует границы лабиринта на сцене
        /// </summary>
        private void OnDrawGizmos()
        {
            if (mazeConfigProvider == null || mazeRenderer == null) return;

            var settings = mazeConfigProvider.GetMazeSettings();

            Gizmos.color = Color.green;

            float cellSize = mazeRenderer.CellSize;
            
            // Размер лабиринта (в клетках)
            float width = settings.MazeCols * cellSize;
            float height = settings.MazeRows * cellSize;

            // Центр куба (чтобы он рисовался не от transform.position)
            Vector3 center = transform.position + new Vector3(width / 2f, height / 2f, 0);

            Vector3 size = new Vector3(width, height, 0);

            Gizmos.DrawWireCube(center, size);
        }
    }
}