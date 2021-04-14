using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlissfulMaze.Common.Maze
{
    public class SimpleMazeGenerationAlgorithm : IMazeGenerationAlgorithm
    {
        public IEnumerable<Vector2> GetNeighbours(IMaze maze, bool[,] visitedCells, int x, int y) // Получаем соседа текущей клетки
        {
            const int distance = 2;
            Vector2[] possibleNeighbours = new[] // Список всех возможных соседeй
            {
                new Vector2(x, y - distance), // Up
                new Vector2(x + distance, y), // Right
                new Vector2(x, y + distance), // Down
                new Vector2(x - distance, y) // Left
            };

            return possibleNeighbours.Where(neighbour =>
                IsNeighbourInsideMaze(maze, neighbour) &&
                IsNeighbourIsCanToVisit(maze, visitedCells, neighbour));
        }

        private static bool IsNeighbourIsCanToVisit(IMaze maze, bool[,] visitedCells, Vector2 neighbour)
        {
            return maze.Cells[(int)neighbour.x, (int)neighbour.y].HasFlag(TypeMazeCell.Empty) && !visitedCells[(int)neighbour.x, (int)neighbour.y];
        }

        private bool IsNeighbourInsideMaze(IMaze maze, Vector2 neighboour)
        {
            return neighboour.x > 0 && neighboour.x < maze.Width && neighboour.y > 0 && neighboour.y < maze.Height;
        }

        private Vector2 ChooseNeighbour(IEnumerable<Vector2> neighbours) //выбор случайного соседа
        {
            int index = Random.Range(0, neighbours.Count());
            return neighbours.ElementAt(index);
        }

        private void RemoveWall(IMaze maze, ref bool[,] visitedCells, Vector2 first, Vector2 second)
        {
            int xDiff = (int)(second.x - first.x);
            int yDiff = (int)(second.y - first.y);
            int addX = (xDiff != 0) ? xDiff / Mathf.Abs(xDiff) : 0; // Узнаем направление удаления стены
            int addY = (yDiff != 0) ? yDiff / Mathf.Abs(yDiff) : 0;
            // Координаты удаленной стены
            maze.Cells[(int)first.x + addX, (int)first.y + addY] = TypeMazeCell.Empty; //обращаем стену в клетку
            visitedCells[(int)first.x + addX, (int)first.y + addY] = true; //и делаем ее посещенной
            visitedCells[(int)second.x, (int)second.y] = true; //делаем клетку посещенной
        }

        public void CreateMaze(IMaze maze)
        {
            bool[,] visitedCells = new bool[maze.Height, maze.Width];
            Stack<Vector2> _path = new Stack<Vector2>();
            _path.Push(new Vector2(1, 1));

            while (_path.Count != 0) //пока в стеке есть клетки ищем соседей и строим путь
            {
                var neighbours = GetNeighbours(maze, visitedCells, (int)_path.Peek().x, (int)_path.Peek().y);
                if (neighbours.Count() != 0)
                {
                    var nextCell = ChooseNeighbour(neighbours);
                    RemoveWall(maze, ref visitedCells, _path.Peek(), nextCell);
                    visitedCells[(int)nextCell.x, (int)nextCell.y] = true; //делаем текущую клетку посещенной
                    _path.Push(nextCell); //затем добавляем её в стек
                }
                else
                {
                    _path.Pop();
                }
            }
        }

        public IMaze Create(int width, int height)
        {
            var maze = new Maze();
            maze.Cells = new TypeMazeCell[height, width];
            maze.Width = width;
            maze.Height = height;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze.Cells[i, j] = TypeMazeCell.Wall;

                    if ((i % 2 != 0 && j % 2 != 0) &&
                        (i < height - 1 && j < width - 1))
                        maze.Cells[i, j] = TypeMazeCell.Empty;
                    else
                        maze.Cells[i, j] = TypeMazeCell.Wall;
                }
            }

            CreateMaze(maze);

            return maze;
        }
    }
}