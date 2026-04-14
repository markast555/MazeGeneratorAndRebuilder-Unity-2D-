using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeSystem.Core;
using MazeSystem.Generation;
using UnityEngine.Serialization;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Отвечает за создание и управление визуальным представлением лабиринта
    /// </summary>
    public class MazeRenderer : MonoBehaviour, IMazeGeneratorListener
    {
        [FormerlySerializedAs("settingsProvider")]
        [Header("Settings")]
        [SerializeField] private MazeConfigProvider configProvider;

        [Header("Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;

        [Header("Config")]
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private float wallThickness = 0.1f;
        [SerializeField] private float wallLength = 1.1f;
        
        public float CellSize => cellSize;
        
        [Header("BuildMode")]
        [SerializeField] private RenderMode buildMode;
        [SerializeField] private float buildDelay = 0.1f;
        
        [Header("GenerationMode")]
        [SerializeField] private RenderMode generationMode;
        [SerializeField] private float generationDelay = 0.1f;

        private Maze _maze;
        private Dictionary<Cell, CellViewData> _view = new();
        
        private Coroutine _buildCoroutine;
        
        private IMazeGenerator _generator;

        private void Awake()
        {
            _generator = new GrowingTreeGenerator();
        }

        #region Build

        /// <summary>
        /// Запускает постройку лабиринта
        /// </summary>
        public void Build()
        {
            InitializeMaze();

            if (_buildCoroutine != null)
                StopCoroutine(_buildCoroutine);

            switch (buildMode)
            {
                case RenderMode.Instant:
                    BuildInstant();
                    break;

                case RenderMode.Animated:
                    _buildCoroutine = StartCoroutine(BuildCoroutine());
                    break;
            }

            switch (generationMode)
            {
                case RenderMode.Instant:
                    _generator.Generate(_maze, this);
                    break;

                case RenderMode.Animated:
                    StartCoroutine(_generator.GenerateAnimated(_maze, this, generationDelay));
                    break;
            }
            
        }
        
        /// <summary>
        /// Инициализирует лабиринт
        /// </summary>
        private void InitializeMaze()
        {
            Clear();

            var settings = configProvider.GetSettings();

            _maze = new Maze(settings.MazeRows, settings.MazeCols);
            MazeBuilder.InitMaze(_maze);

            CreateViewData();
        }
        
        /// <summary>
        /// Строит начальную сетку лабиринта без пауз
        /// </summary>
        private void BuildInstant()
        {
            CreateFloors();
            CreateWalls();
            CreateOuterBorders();
        }
        
        /// <summary>
        /// Строит начальную сетку лабиринта с паузами
        /// </summary>
        /// <returns> Coroutine для поэтапной отрисовки лабиринта </returns>
        private IEnumerator BuildCoroutine()
        {
            foreach (var cell in _maze.AllCells())
            {
                CreateFloor(cell);

                yield return new WaitForSeconds(buildDelay);
            }
            
            foreach (var cell in _maze.AllCells())
            {
                CreateWall(cell, BorderSide.Top);
                CreateWall(cell, BorderSide.Right);

                yield return new WaitForSeconds(buildDelay);
            }

            CreateOuterBorders();
        }

        #endregion

        #region Create

        /// <summary>
        /// Создаёт объекты CellViewData
        /// </summary>
        private void CreateViewData()
        {
            foreach (var cell in _maze.AllCells())
            {
                _view[cell] = new CellViewData();
            }
        }

        /// <summary>
        /// Создаёт пол лабиринта
        /// </summary>
        private void CreateFloors()
        {
            foreach (var cell in _maze.AllCells())
            {
                CreateFloor(cell);
            }
        }
        
        /// <summary>
        /// Создаёт пол для указанной ячейки
        /// </summary>
        /// <param name="cell"> Ячейка </param>
        private void CreateFloor(Cell cell)
        {
            Vector3 pos = GetWorldPosition(cell);

            var floor = Instantiate(floorPrefab, pos, Quaternion.identity, transform);

            floor.transform.localScale = new Vector3(cellSize, cellSize, 1f);

            _view[cell].Floor = floor;
        }

        /// <summary>
        /// Создаёт стены лабиринта
        /// </summary>
        private void CreateWalls()
        {
            foreach (var cell in _maze.AllCells())
            {
                // создаём только TOP и RIGHT — чтобы не было дублей
                CreateWall(cell, BorderSide.Top);
                CreateWall(cell, BorderSide.Right);
            }
        }
        
        /// <summary>
        /// Создаёт стену для указанной ячейки и стороны
        /// </summary>
        /// <param name="cell"> Ячейка </param>
        /// <param name="side"> Сторона  границы </param>
        private void CreateWall(Cell cell, BorderSide side)
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
            _view[cell].Walls[side] = wall;

            // сосед (если есть)
            if (cell.Neighbors.TryGetValue(side, out var neighbor) && neighbor != null)
            {
                var opposite = side.GetOpposite();
                _view[neighbor].Walls[opposite] = wall;
            }
        }
        
        /// <summary>
        /// Создаёт внешние границы лабиринта (нижнюю и левую)
        /// </summary>
        private void CreateOuterBorders()
        {
            int rows = _maze.Rows;
            int cols = _maze.Cols;

            // 🔹 Нижняя граница
            for (int col = 0; col < cols; col++)
            {
                var cell = _maze.GetCell(0, col);

                Vector3 pos = GetWorldPosition(cell) + new Vector3(0, -cellSize / 2f, 0);
                
                var wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                
                wall.transform.localScale = new Vector3(cellSize * wallLength, wallThickness, 1f);
                
                _view[cell].Walls[BorderSide.Bottom] = wall;
            }

            // 🔹 Левая граница
            for (int row = 0; row < rows; row++)
            {
                var cell = _maze.GetCell(row, 0);

                Vector3 pos = GetWorldPosition(cell) + new Vector3(-cellSize / 2f, 0, 0);
                
                var wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                
                wall.transform.localScale = new Vector3(wallThickness, cellSize * wallLength, 1f);
                
                _view[cell].Walls[BorderSide.Left] = wall;
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Удаляет стену у указанной ячейки и обновляет соседа
        /// </summary>
        public void RemoveWall(Cell cell, BorderSide side)
        {
            if (!_view[cell].Walls.TryGetValue(side, out var wall))
                return;

            var neighbor = cell.Neighbors[side];

            _view[cell].Walls.Remove(side);

            if (neighbor != null)
            {
                var opposite = side.GetOpposite();
                _view[neighbor].Walls.Remove(opposite);
            }

            Destroy(wall);
        }

        /// <summary>
        /// Изменяет цвет пола указанной ячейки
        /// </summary>
        public void SetFloorColor(Cell cell, Color color)
        {
            var spriteRenderer = _view[cell].Floor.GetComponent<SpriteRenderer>();

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
        /// <param name="cell"> Ячейка </param>
        /// <returns> Координаты </returns>
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
        private void Clear()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }

            _view.Clear();
        }

        #endregion
        
        public void OnWallRemoved(Cell cell, BorderSide side)
        {
            RemoveWall(cell, side);
        }

        public void OnFloorRepaint(Cell cell, Color color)
        {
            SetFloorColor(cell, color);
        }
        
    }
}