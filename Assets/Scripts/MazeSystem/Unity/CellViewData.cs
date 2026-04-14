using UnityEngine;
using System.Collections.Generic;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Хранит ссылки на визуальные объекты ячейки
    /// </summary>
    public class CellViewData
    {
        public GameObject Floor;

        public Dictionary<BorderSide, GameObject> Walls = new();
    }
}