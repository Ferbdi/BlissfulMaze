namespace BlissfulMaze.Common
{
    public interface IMazeGenerator
    {
        void SetAlgorithm(IMazeGenerationAlgorithm algorithm);
        IMaze Generate(int width, int height);
    }
}