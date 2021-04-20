using UnityEngine;

namespace BlissfulMaze.Common.Maze
{
    public interface IMazePlacementService
    {
        bool IsMoving { get; }
        PlacementState PlacementState { get; }
        ITriggerHandler FinishTrigger { get; }

        void Setup(IMaze maze, MazePlacementSettings mazePlacementSettings, Transform container);
        void MoveUp();
        void MoveDown();
    }
}