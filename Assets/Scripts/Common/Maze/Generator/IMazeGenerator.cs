using Vector2Int = UnityEngine.Vector2Int;

namespace BlissfulMaze.Common.Maze
{
    public interface IMazeGenerator
    {
        void SetAlgorithm(IMazeGenerationAlgorithm algorithm);
        IMaze Generate(int width, int height, Vector2Int finishPosition);
    }
}