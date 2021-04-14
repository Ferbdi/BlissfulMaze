using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BlissfulMaze.Common.Maze
{
    public enum PlacementState
    {
        Down,
        Up
    }

    public class MazePlacementService : IMazePlacementService
    {
        private bool _isMoving;
        private PlacementState _placementState;
        private List<Transform> cells = new List<Transform>();
        private Transform _finishTriggerTransform;
        private ITriggerHandler _finishTriggerHandler;
 
        public bool IsMoving => _isMoving;
        public PlacementState PlacementState => _placementState;
        public ITriggerHandler FinishTrigger => _finishTriggerHandler;

        public void Instantiate(IMaze maze, MazePlacementSettings mazePlacementSettings, Transform container)
        {
            for (int i = 0; i < maze.Height; i++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    if (maze.Cells[i, j].HasFlag(TypeMazeCell.Wall))
                        cells.Add(GameObject.Instantiate(mazePlacementSettings.MazeCellPrefab, new Vector3(-(maze.Height / 2) + i, 0, -(maze.Width / 2) + j), Quaternion.identity, container).transform);
                }
            }

            var trigger = GameObject.Instantiate(mazePlacementSettings.MazeFinishTriggerPrefab, new Vector3(-(maze.Height / 2), 1, -(maze.Width / 2)), Quaternion.identity, container);
            _finishTriggerHandler = trigger.GetComponent<TriggerHandler>();
        }

        public void MoveUp(MazePlacementSettings mazePlacementSettings)
        {
            MoveCellsAsync(mazePlacementSettings, PlacementState.Up, 1);
        }

        public void MoveDown(MazePlacementSettings mazePlacementSettings)
        {
            MoveCellsAsync(mazePlacementSettings, PlacementState.Down, 0);
        }

        private async void MoveCellsAsync(MazePlacementSettings mazePlacementSettings, PlacementState state, float targetY)
        {
            if (_isMoving) return;
            _placementState = state;
            _isMoving = true;

            int counter = 0;
            var tasks = new List<Task>();
            foreach (var cell in cells)
            {
                var targetPosition = new Vector3(cell.transform.position.x, targetY, cell.transform.position.z);
                var task = MoveCellAsync(cell.transform, mazePlacementSettings, targetPosition, (counter + 1f) / (cells.Count() + 1f));
                tasks.Add(task);
                counter++;
            }

            await Task.WhenAll(tasks);
            _isMoving = false;
        }

        private async Task MoveCellAsync(Transform cell, MazePlacementSettings mazePlacementSettings, Vector3 targetPosition, float factor)
        {
            try
            {
                await Task.Delay((int)(1000f * factor));
                while (cell.position != targetPosition)
                {
                    cell.position = Vector3.MoveTowards(cell.position, targetPosition, Time.deltaTime * mazePlacementSettings.SpeedOfPlacementUp * mazePlacementSettings.PlacementCurve.Evaluate(factor));
                    await Task.Yield();
                }
            }
            catch
            {
                Debug.LogWarning("[WARNING] Cell is missing. Maybe scene is reloaded.");
            }
        }
    }
}