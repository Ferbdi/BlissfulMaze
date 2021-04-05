using UnityEngine;

namespace BlissfulMaze.Common.Maze
{
    public interface IMazeInstantiator
    {
        void InstantiateRoutine(IMaze maze, GameObject mazeCellPrefab, Transform container);
    }
}