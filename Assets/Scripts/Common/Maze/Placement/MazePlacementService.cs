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

        private List<Transform> _cells = new List<Transform>();
        private MazePlacementFinish _mazePlacementFinish;

        private MazePlacementSettings _mazePlacementSettings;
        private MazePlacementCell.Pool _mazePlacementCellPool;
        private MazePlacementFinish.Pool _mazePlacementFinishPool;

        public bool IsMoving => _isMoving;
        public PlacementState PlacementState => _placementState;
        public ITriggerHandler FinishTrigger => _mazePlacementFinish?.GetComponent<TriggerHandler>();

        public MazePlacementService(MazePlacementCell.Pool mazePlacementCellPool, MazePlacementFinish.Pool mazePlacementFinishPool)
        {
            _mazePlacementCellPool = mazePlacementCellPool;
            _mazePlacementFinishPool = mazePlacementFinishPool;
        }

        public void Setup(IMaze maze, MazePlacementSettings mazePlacementSettings, Transform container)
        {
            _mazePlacementSettings = mazePlacementSettings;
            CreateCellsFromPoolFor(maze);
        }

        private void CreateFinishFromPool(Vector3 position)
        {
            DespawnMazePlacementFinish();
            _mazePlacementFinish = _mazePlacementFinishPool.Spawn(position);
        }

        private void DespawnMazePlacementFinish()
        {
            if (_mazePlacementFinish == null) return;
            _mazePlacementFinishPool.Despawn(_mazePlacementFinish);
            _mazePlacementFinish = null;
        }

        private void CreateCellsFromPoolFor(IMaze maze)
        {
            _cells.ForEach(c => _mazePlacementCellPool.Despawn(c.GetComponent<MazePlacementCell>()));
            _cells.Clear();
            for (int i = 0; i < maze.Height; i++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    if (maze.Cells[i, j].HasFlag(TypeMazeCell.Wall))
                    {
                        var cell = _mazePlacementCellPool
                            .Spawn(new Vector3(-(maze.Height / 2) + i, 0, -(maze.Width / 2) + j));
                        _cells.Add(cell.transform);
                    }
                    else if (maze.Cells[i, j].HasFlag(TypeMazeCell.Finish))
                    {
                        CreateFinishFromPool(new Vector3(-(maze.Width / 2) + i, 1, -(maze.Height / 2) + j));
                    }
                }
            }
        }

        public void MoveUp()
        {
            MoveCellsAsync(_mazePlacementSettings, PlacementState.Up, 1);
        }

        public void MoveDown()
        {
            DespawnMazePlacementFinish();
            MoveCellsAsync(_mazePlacementSettings, PlacementState.Down, 0);
        }

        private async void MoveCellsAsync(MazePlacementSettings mazePlacementSettings, PlacementState state, float targetY)
        {
            if (_isMoving) return;
            _placementState = state;
            _isMoving = true;

            int counter = 0;
            var tasks = new List<Task>();
            foreach (var cell in _cells)
            {
                var targetPosition = new Vector3(cell.transform.position.x, targetY, cell.transform.position.z);
                var factor = (counter + 1f) / (_cells.Count() + 1f);
                var task = MoveCellAsync(cell.transform, mazePlacementSettings, targetPosition, factor);
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
                    cell.position = Vector3.MoveTowards(cell.position, targetPosition, Time.deltaTime * mazePlacementSettings.MoveSpeed * mazePlacementSettings.PlacementCurve.Evaluate(factor));
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