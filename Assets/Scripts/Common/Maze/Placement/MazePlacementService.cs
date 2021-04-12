using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BlissfulMaze.Common.Maze
{
    public class MazePlacementService : IMazePlacementService
    {
        //void Instantiate();
        //void ReInstantiate();
        //void Up();
        //void Down();


        public void InstantiateRoutine(MazeBehaviour mazeBehaviour, IMaze maze, MazePlacementSettings mazePlacementSettings, Transform container)
        {
            var bufferCells = new List<GameObject>();

            for (int i = 0; i < maze.Height; i++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    if (maze.Cells[i, j].HasFlag(TypeMazeCell.Wall))
                        bufferCells.Add(GameObject.Instantiate(mazePlacementSettings.MazeCellPrefab, new Vector3(-(maze.Height / 2) + i, 0, -(maze.Width / 2) + j), Quaternion.identity, container));
                }
            }

            var trigger = GameObject.Instantiate(mazePlacementSettings.MazeFinishTriggerPrefab, new Vector3(-(maze.Height / 2), 1, -(maze.Width / 2)), Quaternion.identity, container);
            mazeBehaviour.FinishTrigger = trigger.GetComponent<TriggerHandler>();

            int counter = 0;
            foreach (var cell in bufferCells)
            {
                mazeBehaviour.StartCoroutine(CellUpRoutine(mazePlacementSettings, cell.transform, (counter + 1f) / (bufferCells.Count + 1f)));
                counter++;
            }
        }

        private IEnumerator CellUpRoutine(MazePlacementSettings mazePlacementSettings, Transform cell, float factor)
        {
            yield return new WaitForSeconds(1f * factor);
            var targetPosition = new Vector3(cell.position.x, 1, cell.position.z);
            while (cell.position != targetPosition)
            {
                cell.position = Vector3.Lerp(cell.position, targetPosition, Time.deltaTime * mazePlacementSettings.SpeedOfPlacementUp * mazePlacementSettings.PlacementCurve.Evaluate(factor));
                yield return null;
            }
        }
    }
}