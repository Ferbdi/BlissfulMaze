using System;
using System.Collections.Generic;
using System.Linq;
using Vector2Int = UnityEngine.Vector2Int;
using Random = UnityEngine.Random;

namespace BlissfulMaze.Common.Maze
{
    public class SimpleMazeGenerationAlgorithm : IMazeGenerationAlgorithm
    {
        private IEnumerable<Vector2Int> GetNeighbours(IMaze maze, bool[,] visitedCells, int x, int y) // Получаем соседа текущей клетки
        {
            const int distance = 2;
            Vector2Int[] possibleNeighbours = new[] // Список всех возможных соседeй
            {
                new Vector2Int(x, y - distance), // Up
                new Vector2Int(x + distance, y), // Right
                new Vector2Int(x, y + distance), // Down
                new Vector2Int(x - distance, y) // Left
            };

            return possibleNeighbours.Where(neighbour =>
                IsNeighbourInsideMaze(maze, neighbour) &&
                IsNeighbourIsCanToVisit(maze, visitedCells, neighbour));
        }

        private bool IsNeighbourIsCanToVisit(IMaze maze, bool[,] visitedCells, Vector2Int neighbour)
        {
            return maze.Cells[neighbour.x, neighbour.y].HasFlag(TypeMazeCell.Empty) && !visitedCells[neighbour.x, neighbour.y];
        }

        private bool IsNeighbourInsideMaze(IMaze maze, Vector2Int neighbour)
        {
            return neighbour.x > 0 && neighbour.x < maze.Width && neighbour.y > 0 && neighbour.y < maze.Height;
        }

        private Vector2Int ChooseRandomNeighbour(IEnumerable<Vector2Int> neighbours) //выбор случайного соседа
        {
            int index = Random.Range(0, neighbours.Count());
            return neighbours.ElementAt(index);
        }

        private void RemoveWall(IMaze maze, ref bool[,] visitedCells, Vector2Int first, Vector2Int second)
        {
            int xDiff = second.x - first.x;
            int yDiff = second.y - first.y;
            int addX = (xDiff != 0) ? xDiff / Math.Abs(xDiff) : 0; // Узнаем направление удаления стены
            int addY = (yDiff != 0) ? yDiff / Math.Abs(yDiff) : 0;
            // Координаты удаленной стены
            maze.Cells[first.x + addX, first.y + addY] = TypeMazeCell.Empty; //обращаем стену в клетку
            visitedCells[first.x + addX, first.y + addY] = true; //и делаем ее посещенной
            visitedCells[second.x, second.y] = true; //делаем клетку посещенной
        }

        private void GenerateMaze(IMaze maze, Vector2Int finishPosition)
        {
            bool[,] visitedCells = new bool[maze.Height, maze.Width];
            Stack<Vector2Int> _path = new Stack<Vector2Int>();
            _path.Push(finishPosition);

            while (_path.Count != 0) //пока в стеке есть клетки ищем соседей и строим путь
            {
                var neighbours = GetNeighbours(maze, visitedCells, _path.Peek().x, _path.Peek().y);
                if (neighbours.Count() != 0)
                {
                    var nextCell = ChooseRandomNeighbour(neighbours);
                    RemoveWall(maze, ref visitedCells, _path.Peek(), nextCell);
                    visitedCells[nextCell.x, nextCell.y] = true; //делаем текущую клетку посещенной
                    _path.Push(nextCell); //затем добавляем её в стек
                }
                else
                {
                    _path.Pop();
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