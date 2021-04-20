using System;
using System.Collections.Generic;
using System.Linq;
using Vector2Int = UnityEngine.Vector2Int;
using Random = UnityEngine.Random;

namespace BlissfulMaze.Common.Maze
{
    public class SimpleMazeGenerationAlgorithm : IMazeGenerationAlgorithm
    {
        private IEnumerable<Vector2Int> GetNeighbours(IMaze maze, bool[,] visitedCells, Vector2Int position)
        {
            const int distance = 2;
            Vector2Int[] possibleNeighbours = new[]
            {
                new Vector2Int(position.x, position.y - distance), // Up
                new Vector2Int(position.x + distance, position.y), // Right
                new Vector2Int(position.x, position.y + distance), // Down
                new Vector2Int(position.x - distance, position.y) // Left
            };

            return possibleNeighbours.Where(neighbour =>
                IsNeighbourInsideMaze(maze, neighbour) &&
                IsNeighbourIsCanToVisit(maze, visitedCells, neighbour));
        }

        private bool IsNeighbourIsCanToVisit(IMaze maze, bool[,] visitedCells, Vector2Int neighbour)
        {
            return maze.Cells[neighbour.y, neighbour.x].HasFlag(TypeMazeCell.Empty) && !visitedCells[neighbour.y, neighbour.x];
        }

        private bool IsNeighbourInsideMaze(IMaze maze, Vector2Int neighbour)
        {
            return neighbour.x > 0 && neighbour.x < maze.Width && neighbour.y > 0 && neighbour.y < maze.Height;
        }

        private Vector2Int ChooseRandomNeighbour(IEnumerable<Vector2Int> neighbours)
        {
            int index = Random.Range(0, neighbours.Count());
            return neighbours.ElementAt(index);
        }

        private void RemoveWall(IMaze maze, ref bool[,] visitedCells, Vector2Int first, Vector2Int second)
        {
            int xDiff = second.x - first.x;
            int yDiff = second.y - first.y;
            int addX = (xDiff != 0) ? xDiff / Math.Abs(xDiff) : 0;
            int addY = (yDiff != 0) ? yDiff / Math.Abs(yDiff) : 0;

            maze.Cells[first.y + addY, first.x + addX] = TypeMazeCell.Empty;
            visitedCells[first.y + addY, first.x + addX] = true;
            visitedCells[second.y, second.x] = true;
        }

        private void GenerateMaze(IMaze maze, Vector2Int finishPosition)
        {
            bool[,] visitedCells = new bool[maze.Height, maze.Width];
            Stack<Vector2Int> path = new Stack<Vector2Int>();
            path.Push(finishPosition);

            while (path.Count != 0)
            {
                var neighbours = GetNeighbours(maze, visitedCells, path.Peek());
                if (neighbours.Count() != 0)
                {
                    var nextCell = ChooseRandomNeighbour(neighbours);
                    RemoveWall(maze, ref visitedCells, path.Peek(), nextCell);
                    visitedCells[nextCell.y, nextCell.x] = true;
                    path.Push(nextCell);
                }
                else
                {
                    path.Pop();
                }
            }
        }

        private void InitializeMaze(IMaze maze)
        {
            maze.Cells = new TypeMazeCell[maze.Height, maze.Width];

            for (int i = 0; i < maze.Height; i++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    maze.Cells[i, j] = TypeMazeCell.Wall;

                    if ((i % 2 != 0 && j % 2 != 0) &&
                        (i < maze.Height - 1 && j < maze.Width - 1))
                        maze.Cells[i, j] = TypeMazeCell.Empty;
                    else
                        maze.Cells[i, j] = TypeMazeCell.Wall;
                }
            }
        }

        private void SetFinishCellToMaze(IMaze maze, Vector2Int finishPosition)
        {
            maze.Cells[finishPosition.y, finishPosition.x] = TypeMazeCell.Finish;
        }

        public IMaze Create(int width, int height, Vector2Int finishPosition)
        {
            var maze = new Maze();
            maze.Width = width;
            maze.Height = height;

            InitializeMaze(maze);
            GenerateMaze(maze, finishPosition);
            SetFinishCellToMaze(maze, finishPosition);

            return maze;
        }

    }
}