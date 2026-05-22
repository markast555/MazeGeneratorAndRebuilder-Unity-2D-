using System;
using System.Collections;
using MazeSystem.Core;
using MazeSystem.Generation;
using MazeSystem.Unity.Debugging;
using UnityEngine;


namespace MazeSystem.Unity
{
    /// <summary>
    /// Управляет генерацией и отображением лабиринта
    /// </summary>
    public class MazeController : MonoBehaviour, IMazeGeneratorListener, ISafeZoneListener
    {
        [Header("Scripts")]
        [SerializeField] private MazeConfigProvider mazeConfigProvider;
        [SerializeField] private MazeRenderer mazeRenderer;
        
        [Header("Animated Settings")]
        [SerializeField] private float delay = 0.1f;
        
        private Maze _maze;
        private MazeViewMap _view;
        private SafeZone _safeZone;
        private MazeRebuilder _rebuilder;
        private IMazeGenerator _generator;
        private ISafeZoneGenerator _safeZoneGenerator;
        
        private IPositionProvider _positionProvider;
        private MazeCoordinateHelper _mazeCoordinateHelper;
        
        /// <summary>
        /// Инициализирует зависимости контроллера
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Не заданы необходимые компоненты контроллера
        /// </exception>
        private void Awake()
        {
            if (mazeConfigProvider == null)
                throw new InvalidOperationException("ConfigProvider not set");

            if (mazeRenderer == null)
                throw new InvalidOperationException("MazeRenderer not set");
            
            _generator = new GrowingTreeGenerator();
            _rebuilder = new MazeRebuilder();
        }
        
        /// <summary>
        /// Устанавливает источник позиции
        /// </summary>
        /// <param name="provider">Источник позиции</param>
        public void SetPositionProvider(IPositionProvider provider)
        {
            _positionProvider = provider;
        }
        
        #region Initialization
        
        /// <summary>
        /// Инициализирует лабиринт
        /// </summary>
        public void InitializeMaze()
        {
            mazeRenderer.Clear();
            _view = new MazeViewMap();

            if (_mazeCoordinateHelper == null)
            {
                _mazeCoordinateHelper = new MazeCoordinateHelper(
                    transform,
                    mazeRenderer.CellSize
                );
            }
            
            var settings = mazeConfigProvider.GetMazeSettings();

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
            mazeRenderer.CreateOuterBorders(_maze, _view);
            CreateEnterAndExit();
        }
        
        #endregion
        
        #region Maze Creation
        
        /// <summary>
        /// Создаёт данные визуального представления ячеек
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
                mazeRenderer.CreateFloor(cell, viewData);
            }
        }
        
        /// <summary>
        /// Создаёт стены лабиринта
        /// </summary>
        private void CreateWalls()
        {
            foreach (var cell in _maze.AllCells())
            {
                // Создание только Top и Right, чтобы не было дублей
                CreateWall(cell, BorderSide.Top);
                CreateWall(cell, BorderSide.Right);
            }
        }
        
        /// <summary>
        /// Создаёт конкретную стену лабиринта
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона ячейки</param>
        private void CreateWall(Cell cell, BorderSide side)
        {
            if (!cell.HasWall(side))
                return;

            var viewData = _view.Get(cell);
            mazeRenderer.CreateWall(cell, side, viewData);
            
            DebugLogger.Log($"Create wall: ({cell.Row},{cell.Col}) {side}", DebugCategory.Create);
            
            // Привязка этой же стены соседу, но с противоположной стороны
            if (cell.Neighbors.TryGetValue(side, out var neighbor) && neighbor != null)
            {
                var neighborView = _view.Get(neighbor);
                var opposite = side.GetOpposite();

                neighborView.Walls[opposite] = viewData.Walls[side];
            }
            
            if (neighbor != null)
            {
                DebugLogger.Log(
                    $"Link: ({cell.Row},{cell.Col}) {side} ↔ ({neighbor.Row},{neighbor.Col}) {side.GetOpposite()}",
                    DebugCategory.Neighbors
                    );
            }
        }
        
        /// <summary>
        /// Создаёт вход и выход лабиринта
        /// </summary>
        private void CreateEnterAndExit()
        {
            var middleCol = _maze.Cols / 2;

            var topCell = _maze.GetCell(_maze.Rows - 1,
                middleCol
            );

            OnWallRemoved(
                topCell,
                BorderSide.Top
            );

            var bottomCell = _maze.GetCell(0, middleCol);

            OnWallRemoved(
                bottomCell,
                BorderSide.Bottom
            );
        }
        
        #endregion

        #region Generation
        
        /// <summary>
        /// Генерирует лабиринт в зависимости от режима отображения
        /// </summary>
        /// <param name="mode">Режим</param>
        /// <returns>Enumerator для пошаговой генерации</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый режим отображения лабиринта
        /// </exception>
        public IEnumerator GenerateMaze(RenderMode mode)
        {
            switch (mode)
            {
                case RenderMode.Instant:
                    _generator.Generate(_maze, this);
                    break;

                case RenderMode.Animated:
                    yield return StartCoroutine(
                        _generator.GenerateAnimated(
                            _maze,
                            this,
                            delay
                        )
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(mode), 
                        mode, 
                        "Unsupported RenderMode value");
            }
        }
        
        /// <summary>
        /// Создаёт генератор безопасной зоны
        /// </summary>
        /// <returns>Генератор безопасной зоны</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый режим формирования безопасной зоны
        /// </exception>
        private ISafeZoneGenerator CreateSafeZoneGenerator()
        {
            return mazeConfigProvider.safeZoneMode switch
            {
                SafeZoneMode.Square => new SquareSafeZoneGenerator(),
                SafeZoneMode.Dynamic => new DynamicSafeZoneGenerator(),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(mazeConfigProvider.safeZoneMode), 
                    mazeConfigProvider.safeZoneMode, 
                    "Unsupported SafeZoneMode value")
            };
        }
        
        /// <summary>
        /// Формирует безопасную зону
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Источник не установлен
        /// </exception>
        public bool GenerateSafeZone()
        {
            if (_positionProvider == null)
                throw new InvalidOperationException("PositionProvider is not set");

            if (_maze == null)
                throw new InvalidOperationException("Maze is not initialized");

            if (_mazeCoordinateHelper == null)
                throw new InvalidOperationException("MazeCoordinateConverter is not initialized");
            
            var worldPos = _positionProvider.GetWorldPosition();
            
            DebugLogger.Log($"Player position: {worldPos.ToString()}", DebugCategory.Player);
            if (!_mazeCoordinateHelper.TryGetCell(worldPos, _maze, out var cell))
            {
                DebugLogger.Log("Player is not in the maze", DebugCategory.Player);
                return false;
            }

            var context = new SafeZoneContext
            {
                Maze = _maze,
                PlayerPosition = new Vector2Int(cell.Row, cell.Col)
            };
            
            DebugLogger.Log(
                $"Player position in the maze: {context.PlayerPosition.ToString()}", 
                DebugCategory.Player);
            
            _safeZoneGenerator = CreateSafeZoneGenerator();

            var settings = mazeConfigProvider.GetSafeZoneSettings();
            _safeZone = _safeZoneGenerator.Generate(context, settings, this);
            
            return true;
        }

        /// <summary>
        /// Перестраивает лабиринт в зависимости от режима отображения
        /// </summary>
        /// <param name="mode">Режим</param>
        /// <returns>Enumerator для пошаговой генерации</returns>
        /// <exception cref="InvalidOperationException">
        /// Генератор лабиринта не инициализирован
        /// или не поддерживает перестройку
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый режим отображения лабиринта
        /// </exception>
        public IEnumerator RebuildMaze(RenderMode mode)
        {
            if (_generator == null)
                throw new InvalidOperationException("Generator is not initialized");
            
            if (_generator is not IContinuableMazeGenerator continuable)
                throw new InvalidOperationException(
                    "Generator does not support rebuild");

            switch (mode)
            {
                case RenderMode.Instant:
                    _rebuilder.Rebuild(
                        _maze,
                        _safeZone,
                        continuable,
                        this
                    );
                    break;

                case RenderMode.Animated:
                    yield return StartCoroutine(
                        _rebuilder.RebuildAnimated(
                            _maze,
                            _safeZone,
                            continuable,
                            this,
                            delay
                        )
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(mode), 
                        mode, 
                        "Unsupported RenderMode value");
            }
        }
        
        #endregion
        
        #region Event Handlers
        
        /// <summary>
        /// Обрабатывает создание стены
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона ячейки</param>
        public void OnWallCreated(Cell cell, BorderSide side)
        {
            var viewData = _view.Get(cell);

            mazeRenderer.CreateWall(cell, side, viewData);

            if (cell.Neighbors.TryGetValue(side, out var neighbor) && neighbor != null)
            {
                var neighborView = _view.Get(neighbor);
                neighborView.Walls[side.GetOpposite()] = viewData.Walls[side];
            }
        }
        
        /// <summary>
        /// Обрабатывает удаление стены
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="side">Сторона ячейки</param>
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

            mazeRenderer.DestroyWall(wall);
        }
        
        /// <summary>
        /// Обрабатывает перекраску ячейки (пола)
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="color">Цвет</param>
        public void OnFloorRepaint(Cell cell, Color color)
        {
            var viewData = _view.Get(cell);
            mazeRenderer.SetFloorColor(color, viewData);
        }
        
        #endregion
    }
}