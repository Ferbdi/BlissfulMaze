using UnityEngine;

namespace BlissfulMaze.Common
{
    public interface IMazeInstantiator
    {
        void InstantiateRoutine(IMaze maze, GameObject mazeCellPrefab, Transform container);
    }
}