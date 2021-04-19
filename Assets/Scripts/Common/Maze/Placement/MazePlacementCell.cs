using UnityEngine;
using Zenject;

namespace BlissfulMaze.Common.Maze
{
    public class MazePlacementCell : MonoBehaviour
    {
        public class Pool : MonoMemoryPool<Vector3, MazePlacementCell>
        {
            protected override void Reinitialize(Vector3 position, MazePlacementCell mazePlacementCell)
            {
                mazePlacementCell.transform.position = position;
            }
        }
    }
}