using UnityEngine;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    public class MazeRunner : MonoBehaviour
    {
        [SerializeField] private MazeSettingsProvider settingsProvider;

        private void Start()
        {
            MazeSettings settings;

            try
            {
                settings = settingsProvider.CreateSettings();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Invalid maze settings: {e.Message}");
                return;
            }
            
            Debug.Log($"Maze: {settings.MazeRows}x{settings.MazeCols}");
        }
    }
}