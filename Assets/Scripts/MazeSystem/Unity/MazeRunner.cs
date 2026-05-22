using System;
using UnityEngine;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Запускает выбранный сценарий работы лабиринта
    /// </summary>
    public class MazeRunner : MonoBehaviour
    {
        [SerializeField] private MazeManager mazeManager;
        [SerializeField] private MazeScenario scenario;

        /// <summary>
        /// Запускает выбранный сценарий
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Передан неподдерживаемый тип сценария
        /// </exception>
        private void Start()
        {
            switch (scenario)
            {
                case MazeScenario.SimpleGeneration:
                    mazeManager.GenerateMaze();
                    break;

                case MazeScenario.GenerationWithRebuild:
                    mazeManager.GenerateMazeWithRebuild();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(scenario), 
                        scenario, 
                        "Unsupported MazeScenario value");
            }
        }
    }
}