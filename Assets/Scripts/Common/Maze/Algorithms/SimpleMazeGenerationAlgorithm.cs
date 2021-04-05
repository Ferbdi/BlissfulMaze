namespace BlissfulMaze.Common.Maze
{
    public class SimpleMazeGenerationAlgorithm : IMazeGenerationAlgorithm
    {
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
                        maze.Cells[i, j] = TypeMazeCell.Wall;
                    else
                        maze.Cells[i, j] = TypeMazeCell.Empty;
                }
            }

            return maze;
        }
    }
}