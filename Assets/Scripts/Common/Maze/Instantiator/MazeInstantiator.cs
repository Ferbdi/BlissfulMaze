using UnityEngine;

namespace BlissfulMaze.Common
{
    public class MazeInstantiator : IMazeInstantiator
    {
        public void InstantiateRoutine(IMaze maze, GameObject mazeCellPrefab, Transform container)
        {
            for (int i = 0; i < maze.Height; i++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    if (maze.Cells[i, j].HasFlag(TypeMazeCell.Wall))
                        GameObject.Instantiate(mazeCellPrefab, new Vector3(-(maze.Height / 2) + i, 1, -(maze.Width / 2) + j), Quaternion.identity, container);
                }
            }
        }
    }
}