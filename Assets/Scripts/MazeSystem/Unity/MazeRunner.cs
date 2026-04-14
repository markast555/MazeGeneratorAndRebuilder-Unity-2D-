using UnityEngine;

namespace MazeSystem.Unity
{
    public class MazeRunner : MonoBehaviour
    {
        [SerializeField] private MazeRenderer mazeRenderer;

        private void Start()
        {
            mazeRenderer.Build();
        }
    }
}