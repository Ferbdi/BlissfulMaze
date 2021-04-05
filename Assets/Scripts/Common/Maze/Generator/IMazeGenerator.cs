namespace BlissfulMaze.Common.Maze
{
    public interface IMazeGenerator
    {
        void SetAlgorithm(IMazeGenerationAlgorithm algorithm);
        IMaze Generate(int width, int height);
    }
}