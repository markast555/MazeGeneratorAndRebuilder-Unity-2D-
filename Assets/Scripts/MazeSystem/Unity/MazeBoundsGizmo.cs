using UnityEngine;
using UnityEngine.Serialization;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Класс отрисовки границ лабиринта на сцене
    /// </summary>
    public class MazeBoundsGizmo : MonoBehaviour
    {
        [FormerlySerializedAs("settingsProvider")] [SerializeField] private MazeConfigProvider configProvider;
        [FormerlySerializedAs("renderer")] [SerializeField] private MazeRenderer rendererMaze;
        // [SerializeField] private Grid grid;

        private void OnDrawGizmos()
        {
            if (configProvider == null || rendererMaze == null) return;
            
            // Привязка к сетке
            //Vector3Int cellPos = grid.WorldToCell(transform.position);
            // Vector3 snappedPosition = grid.CellToWorld(cellPos);
            
            // transform.position = snappedPosition;

            var settings = configProvider.GetSettings();

            Gizmos.color = Color.green;

            float cellSize = rendererMaze.CellSize;
            
            // Размер лабиринта (в клетках)
            float width = settings.MazeCols * cellSize;
            float height = settings.MazeRows * cellSize;

            // Центр куба (чтобы он рисовался от transform.position)
            Vector3 center = transform.position + new Vector3(width / 2f, height / 2f, 0);

            Vector3 size = new Vector3(width, height, 0);

            Gizmos.DrawWireCube(center, size);
        }
    }
}