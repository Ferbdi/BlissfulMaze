namespace BlissfulMaze.Common.Maze
{
    public interface IMazeGenerationAlgorithm
    {
        IMaze Create(int width, int height);
    }
}