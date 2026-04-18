using UnityEngine;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Отвечает за визуализацию лабиринта
    /// </summary>
    public class MazeRenderer : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;

        [Header("Config")]
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private float wallThickness = 0.1f;
        [SerializeField] private float wallLength = 1.1f;
        
        public float CellSize => cellSize;
        
        // /// <summary>
        // /// Запускает постройку лабиринта
        // /// </summary>
        // public void Build()
        // {
        //     InitializeMaze();
        //
        //     if (_buildCoroutine != null)
        //         StopCoroutine(_buildCoroutine);
        //
        //     switch (buildMode)
        //     {
        //         case RenderMode.Instant:
        //             BuildInstant();
        //             break;
        //
        //         case RenderMode.Animated:
        //             _buildCoroutine = StartCoroutine(BuildCoroutine());
        //             break;
        //     }
        //
        //     switch (generationMode)
        //     {
        //         case RenderMode.Instant:
        //             _generator.Generate(_maze, this);
        //             break;
        //
        //         case RenderMode.Animated:
        //             StartCoroutine(_generator.GenerateAnimated(_maze, this, generationDelay));
        //             break;
        //     }
        //     
        // }
        

        #region Create
        
        /// <summary>
        /// Создаёт пол для указанной ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void CreateFloor(Cell cell, CellViewData viewData)
        {
            Vector3 pos = GetWorldPosition(cell);

            var floor = Instantiate(floorPrefab, pos, Quaternion.identity, transform);

            floor.transform.localScale = new Vector3(cellSize, cellSize, 1f);

            viewData.Floor = floor;
        }
        
        /// <summary>
        /// Создаёт стену для указанной ячейки и стороны
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона  границы</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void CreateWall(Cell cell, BorderSide side, CellViewData viewData)
        {
            if (!cell.HasWall(side))
                return;

            Vector3 basePos = GetWorldPosition(cell);
            Vector3 wallPos = basePos;
            
            var wall = Instantiate(wallPrefab, basePos, Quaternion.identity, transform);

            switch (side)
            {
                case BorderSide.Right:
                    wallPos += new Vector3(cellSize / 2f, 0, 0);
                    wall.transform.localScale = new Vector3(wallThickness, cellSize * wallLength, 1f);
                    break;

                case BorderSide.Top:
                    wallPos += new Vector3(0, cellSize / 2f, 0);
                    wall.transform.localScale = new Vector3(cellSize * wallLength, wallThickness, 1f);
                    break;

                default:
                    Destroy(wall);
                    return;
            }

            wall.transform.position = wallPos;
            
            // текущая ячейка
            viewData.Walls[side] = wall;
            
        }
        
        /// <summary>
        /// Создаёт внешние границы лабиринта (нижнюю и левую)
        /// </summary>
        /// <param name="maze">Лабиринт</param>
        /// <param name="mazeViewMap">
        /// Словарь ячеек и визуальных представлений
        /// </param>
        public void CreateOuterBorders(Maze maze, MazeViewMap mazeViewMap)
        {
            int rows = maze.Rows;
            int cols = maze.Cols;

            // 🔹 Нижняя граница
            for (int col = 0; col < cols; col++)
            {
                var cell = maze.GetCell(0, col);

                Vector3 pos = GetWorldPosition(cell) + new Vector3(0, -cellSize / 2f, 0);
                
                var wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                
                wall.transform.localScale = new Vector3(cellSize * wallLength, wallThickness, 1f);
                
                mazeViewMap.Get(cell).Walls[BorderSide.Bottom] = wall;
            }

            // 🔹 Левая граница
            for (int row = 0; row < rows; row++)
            {
                var cell = maze.GetCell(row, 0);

                Vector3 pos = GetWorldPosition(cell) + new Vector3(-cellSize / 2f, 0, 0);
                
                var wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                
                wall.transform.localScale = new Vector3(wallThickness, cellSize * wallLength, 1f);
                
                mazeViewMap.Get(cell).Walls[BorderSide.Left] = wall;
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Удаляет стену
        /// </summary>
        /// <param name="wall">Стена</param>
        public void DestroyWall(GameObject wall)
        {
            Destroy(wall);
        }

        /// <summary>
        /// Изменяет цвет пола указанной ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="color">Цвет</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void SetFloorColor(Cell cell, Color color, CellViewData viewData)
        {
            var spriteRenderer = viewData.Floor.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Вычисляет мировую позицию центра ячейки лабиринта
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <returns>Координаты</returns>
        private Vector3 GetWorldPosition(Cell cell)
        {
            return transform.position + new Vector3(
                cell.Col * cellSize +  cellSize / 2f,
                cell.Row * cellSize + cellSize / 2f,
                0
            );
        }

        /// <summary>
        /// Удаляет все визуальные объекты лабиринта со сцены
        /// </summary>
        public void Clear()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        #endregion
        
    }
}