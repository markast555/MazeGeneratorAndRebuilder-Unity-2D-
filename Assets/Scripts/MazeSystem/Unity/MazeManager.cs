using System.Collections;
using UnityEngine;
using MazeSystem.Unity.Debugging;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Управляет сценариями модуля
    /// </summary>
    public class MazeManager : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] private MazeController mazeController;
        [SerializeField] private MonoBehaviour positionProvider;
        
        [SerializeField] private DebugConfig debugConfig;

        [Header("Rendering Settings")]
        [SerializeField] private RenderMode renderMode;

        [Header("Auto Regeneration")]
        [SerializeField] private float regenerationInterval = 10f;

        private Coroutine _autoRoutine;

        private bool _isRegenerating;

        /// <summary>
        /// Инициализирует зависимости менеджера
        /// </summary>
        private void Awake()
        {
            mazeController.SetPositionProvider(
                (IPositionProvider)positionProvider
            );

            DebugContext.Config = debugConfig;
        }
        
        /// <summary>
        /// Запускает сценарий генерации лабиринта
        /// </summary>
        public void GenerateMaze()
        {
            StartCoroutine(
                GenerateMazeRoutine()
            );
        }

        /// <summary>
        /// Запускает сценарий генерации лабиринта
        /// с последующей перестройкой
        /// </summary>
        public void GenerateMazeWithRebuild()
        {
            StartCoroutine(
                GenerateMazeWithRebuildRoutine()
            );
        }

        /// <summary>
        /// Запускает автоматическую перестройку лабиринта
        /// </summary>
        private void StartAutoRegeneration()
        {
            if (_autoRoutine != null)
                return;

            _autoRoutine = StartCoroutine(
                AutoRegenerationRoutine()
            );
        }

        /// <summary>
        /// Останавливает автоматическую перестройку лабиринта
        /// </summary>
        private void StopAutoRegeneration()
        {
            if (_autoRoutine != null)
            {
                StopCoroutine(_autoRoutine);
                _autoRoutine = null;
            }
        }
        
        /// <summary>
        /// Выполняет полный сценарий генерации лабиринта
        /// </summary>
        /// <returns>Enumerator coroutine</returns>
        private IEnumerator GenerateMazeRoutine()
        {
            mazeController.InitializeMaze();

            mazeController.Build();

            yield return StartCoroutine(
                mazeController.GenerateMaze(renderMode)
            );
        }

        /// <summary>
        /// Выполняет генерацию лабиринта
        /// и запускает автоматическую перестройку
        /// </summary>
        /// <returns>Enumerator coroutine</returns>
        private IEnumerator GenerateMazeWithRebuildRoutine()
        {
            yield return StartCoroutine(
                GenerateMazeRoutine()
            );

            StartAutoRegeneration();
        }
        
        /// <summary>
        /// Выполняет автоматическую перестройку лабиринта
        /// через заданные интервалы времени 
        /// </summary>
        /// <returns>Enumerator coroutine</returns>
        private IEnumerator AutoRegenerationRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(
                    regenerationInterval
                );

                yield return StartCoroutine(
                    RebuildOnlyRoutine()
                );
            }
        }
        
        /// <summary>
        /// Выполняет перестройку лабиринта
        /// с предварительным формированием безопасной зоны
        /// </summary>
        /// <returns>Enumerator coroutine</returns>
        private IEnumerator RebuildOnlyRoutine()
        {
            if (_isRegenerating)
                yield break;

            _isRegenerating = true;

            bool safeZoneGenerated =
                mazeController.GenerateSafeZone();
            
            // Задержка кадра для фиксации лабиринта с отмеченной безопасной зоной
            // до перестройки
            if (DebugContext.Config != null &&
                DebugContext.Config.IsEnabled(DebugCategory.Screenshot))
            {
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot(
                    "DebugScreens/screen_1_before.png"
                );
                yield return null;
            }

            if (safeZoneGenerated )
            {
                yield return StartCoroutine(
                    mazeController.RebuildMaze(renderMode)
                );

                // Задержка кадра для фиксации лабиринта после перегенерации
                // и восстановления связности
                if (DebugContext.Config != null &&
                    DebugContext.Config.IsEnabled(DebugCategory.Screenshot))
                {
                    yield return new WaitForEndOfFrame();
                    ScreenCapture.CaptureScreenshot(
                        "DebugScreens/screen_4_after.png"
                    );
                    yield return null;
                }
            }

            _isRegenerating = false;
        }
    }
}