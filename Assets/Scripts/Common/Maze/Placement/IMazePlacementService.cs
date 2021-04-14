using UnityEngine;
using System.Collections.Generic;

namespace BlissfulMaze.Common.Maze
{
    public interface IMazePlacementService
    {
        bool IsMoving { get; }
        PlacementState PlacementState { get; }
        ITriggerHandler FinishTrigger { get; }

        void Instantiate(IMaze maze, MazePlacementSettings mazePlacementSettings, Transform container);
        void MoveUp(MazePlacementSettings mazePlacementSettings);
        void MoveDown(MazePlacementSettings mazePlacementSettings);
    }
}