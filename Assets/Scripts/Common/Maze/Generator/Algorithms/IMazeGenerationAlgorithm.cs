using Vector2Int = UnityEngine.Vector2Int;

namespace BlissfulMaze.Common.Maze
{
    public interface IMazeGenerationAlgorithm
    {
        IMaze Create(int width, int height, Vector2Int finishPosition);
    }
}