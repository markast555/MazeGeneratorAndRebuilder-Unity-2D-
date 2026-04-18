using MazeSystem.Core;
using MazeSystem.Generation;
using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Управляет генерацией лабиринта и его визуализацией
    /// </summary>
    public class MazeController : MonoBehaviour, IMazeGeneratorListener
    {
        [Header("Settings")]
        [SerializeField] private MazeConfigProvider configProvider;
        
        [Header("Renderer")]
        [SerializeField] private MazeRenderer rendererMaze;
        
        [Header("GenerationMode")]
        [SerializeField] private float generationDelay = 0.1f;
        
        
        private Maze _maze;
        private MazeViewMap _view;
        
        private Coroutine _buildCoroutine;
        
        private IMazeGenerator _generator;
        
        
        private void Awake()
        {
            _generator = new GrowingTreeGenerator();
        }
        
        /// <summary>
        /// Инициализирует лабиринт
        /// </summary>
        public void InitializeMaze()
        {
            rendererMaze.Clear();
            _view = new MazeViewMap();

            var settings = configProvider.GetMazeSettings();

            _maze = new Maze(settings.MazeRows, settings.MazeCols);
            MazeBuilder.InitMaze(_maze);

            CreateViewData();
        }
        
        
        /// <summary> 
        /// Строит начальную сетку лабиринта
        /// </summary>
        public void Build()
        {
            CreateFloors();
            CreateWalls();
            rendererMaze.CreateOuterBorders(_maze, _view);
        }
        
        /// <summary>
        /// Создаёт объекты CellViewData
        /// </summary>
        private void CreateViewData()
        {
            foreach (var cell in _maze.AllCells())
            {
                _view.Add(cell, new CellViewData());
            }
        }
        
        /// <summary>
        /// Создаёт пол лабиринта
        /// </summary>
        private void CreateFloors()
        {
            foreach (var cell in _maze.AllCells())
            {
                var viewData = _view.Get(cell);
                rendererMaze.CreateFloor(cell, viewData);
            }
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
        /// Создаёт конкретную стену лабиринта
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона</param>
        private void CreateWall(Cell cell, BorderSide side)
        {
            if (!cell.HasWall(side))
                return;

            var viewData = _view.Get(cell);

            rendererMaze.CreateWall(cell, side, viewData);
            // Debug.Log($"CREATE: ({cell.Row},{cell.Col}) {side}");
            // 🔹 СВЯЗЫВАЕМ С СОСЕДОМ ЗДЕСЬ
            if (cell.Neighbors.TryGetValue(side, out var neighbor) && neighbor != null)
            {
                var neighborView = _view.Get(neighbor);
                var opposite = side.GetOpposite();

                neighborView.Walls[opposite] = viewData.Walls[side];
            }
            // if (neighbor != null)
            // {
            //     Debug.Log($"LINK: ({cell.Row},{cell.Col}) {side} ↔ ({neighbor.Row},{neighbor.Col}) {side.GetOpposite()}");
            // }
        }

        /// <summary>
        /// Генерирует лабиринт в зависимости от режима
        /// </summary>
        /// <param name="mode">Режим</param>
        public void Generate(RenderMode mode)
        {
            if (_buildCoroutine != null)
                StopCoroutine(_buildCoroutine);
            
            switch (mode)
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
        /// Событие удаления стены
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона границы</param>
        public void OnWallRemoved(Cell cell, BorderSide side)
        {
            var viewData = _view.Get(cell);

            if (!viewData.Walls.TryGetValue(side, out var wall))
                return;

            viewData.Walls.Remove(side);

            if (cell.Neighbors.TryGetValue(side, out var neighbor) && neighbor != null)
            {
                var neighborView = _view.Get(neighbor);
                var opposite = side.GetOpposite();

                neighborView.Walls.Remove(opposite);
            }

            rendererMaze.DestroyWall(wall);
        }

        /// <summary>
        /// Событие перекарски ячейки (пола)
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="color">Цвет</param>
        public void OnFloorRepaint(Cell cell, Color color)
        {
            var viewData = _view.Get(cell);
            rendererMaze.SetFloorColor(cell, color, viewData);
        }
    }
}