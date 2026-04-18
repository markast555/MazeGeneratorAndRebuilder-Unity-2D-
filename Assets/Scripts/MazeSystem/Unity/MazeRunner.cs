using UnityEngine;

namespace MazeSystem.Unity
{
    public class MazeRunner : MonoBehaviour
    {
        [SerializeField] private MazeManager mazeManager;

        private void Start()
        {
            mazeManager.GenerateMaze();
        }
    }
}