using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Определяет сценарий генерации лабиринта
    /// </summary>
    public class MazeManager: MonoBehaviour
    {
        [SerializeField] private MazeController controller;
        // [SerializeField] private RenderMode buildMode;
        [SerializeField] private RenderMode generationMode;

        /// <summary>
        /// Генерирует лабиринт
        /// </summary>
        public void GenerateMaze()
        {
            controller.InitializeMaze();
            controller.Build();
            controller.Generate(generationMode);
        }
        
        /// <summary>
        /// Генерирует лабиринт с возможностью перестройки
        /// </summary>
        public void GenerateMazeWithRebuild()
        {
            GenerateMaze();
            // TODO
        }
        
    }
}