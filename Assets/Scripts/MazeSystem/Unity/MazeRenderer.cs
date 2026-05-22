using System;
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
        
        private MazeCoordinateHelper _mazeCoordinateHelper;
        
        public float CellSize => cellSize;
        
        /// <summary>
        /// Инициализирует зависимости рендеринга
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Не заданы необходимые компоненты рендеринга
        /// </exception>
        private void Awake()
        {
            if (floorPrefab == null)
                throw new InvalidOperationException("Floor Prefab not set");

            if (wallPrefab == null)
                throw new InvalidOperationException("Wall Prefab not set");
            
            _mazeCoordinateHelper = new MazeCoordinateHelper(transform, cellSize);
        }
        
        /// <summary>
        /// Создаёт пол для указанной ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void CreateFloor(Cell cell, CellViewData viewData)
        {
            var pos = _mazeCoordinateHelper.GetCellCenter(cell);
            var floor = Instantiate(floorPrefab, pos, Quaternion.identity, transform);
            floor.transform.localScale = new Vector3(cellSize, cellSize, 1f);
            viewData.Floor = floor;
        }
        
        /// <summary>
        /// Создаёт стену для указанной стороны ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона ячейки</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void CreateWall(Cell cell, BorderSide side, CellViewData viewData)
        {
            if (!cell.HasWall(side))
                return;

            var basePos = _mazeCoordinateHelper.GetCellCenter(cell);
            var wallPos = basePos;
            var wall = Instantiate(wallPrefab, basePos, Quaternion.identity, transform);

            switch (side)
            {
                case BorderSide.Right:
                    wallPos += new Vector3(cellSize / 2f, 0, 0);
                    wall.transform.localScale = 
                        new Vector3(wallThickness, cellSize * wallLength, 1f);
                    break;

                case BorderSide.Top:
                    wallPos += new Vector3(0, cellSize / 2f, 0);
                    wall.transform.localScale = 
                        new Vector3(cellSize * wallLength, wallThickness, 1f);
                    break;

                default:
                    Destroy(wall);
                    return;
            }

            wall.transform.position = wallPos;
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
            var rows = maze.Rows;
            var cols = maze.Cols;

            // Нижняя граница
            for (int col = 0; col < cols; col++)
            {
                var cell = maze.GetCell(0, col);
                var pos = _mazeCoordinateHelper.GetCellCenter(cell) + 
                          new Vector3(0, -cellSize / 2f, 0);
                var wall = 
                    Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                wall.transform.localScale = 
                    new Vector3(cellSize * wallLength, wallThickness, 1f);
                mazeViewMap.Get(cell).Walls[BorderSide.Bottom] = wall;
            }

            // Левая граница
            for (var row = 0; row < rows; row++)
            {
                var cell = maze.GetCell(row, 0);
                var pos = _mazeCoordinateHelper.GetCellCenter(cell) +
                          new Vector3(-cellSize / 2f, 0, 0);
                var wall = 
                    Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                wall.transform.localScale = 
                    new Vector3(wallThickness, cellSize * wallLength, 1f);
                mazeViewMap.Get(cell).Walls[BorderSide.Left] = wall;
            }
        }

        /// <summary>
        /// Удаляет стену
        /// </summary>
        /// <param name="wall">Стена</param>
        public void DestroyWall(GameObject wall)
        {
            Destroy(wall);
        }

        /// <summary>
        /// Изменяет цвет пола ячейки
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <param name="viewData">
        /// Визуальное представление ячейки
        /// </param>
        public void SetFloorColor(Color color, CellViewData viewData)
        {
            var spriteRenderer = viewData.Floor.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }
        
        /// <summary>
        /// Удаляет все визуальные объекты лабиринта со сцены
        /// </summary>
        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}