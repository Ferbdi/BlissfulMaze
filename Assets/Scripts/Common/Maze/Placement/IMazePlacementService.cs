using UnityEngine;
using System;
using System.Collections;

namespace BlissfulMaze.Common.Maze
{
    public interface IMazePlacementService
    {
        void InstantiateRoutine(MazeBehaviour mazeBehaviour, IMaze maze, MazePlacementSettings mazePlacementSettings, Transform container);
    }
}