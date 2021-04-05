namespace BlissfulMaze.Common
{
    public interface IMazeGenerationAlgorithm
    {
        IMaze Create(int width, int height);
    }
}