using UnityEngine;
using System.Collections.Generic;
using MazeSystem.Core;

namespace MazeSystem.Unity
{
    /// <summary>
    /// Хранит визуальные объекты ячейки лабиринта
    /// </summary>
    public class CellViewData
    {
        /// <summary>
        /// Объект пола ячейки
        /// </summary>
        public GameObject Floor;

        /// <summary>
        /// Объекты стен ячейки по сторонам
        /// </summary>
        public Dictionary<BorderSide, GameObject> Walls = new();
    }
}