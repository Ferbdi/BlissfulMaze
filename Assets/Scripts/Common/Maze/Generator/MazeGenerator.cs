namespace BlissfulMaze.Common.Maze
{
    public class MazeGenerator : IMazeGenerator
    {
        private IMazeGenerationAlgorithm _algorithm;

        public MazeGenerator(IMazeGenerationAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public void SetAlgorithm(IMazeGenerationAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public IMaze Generate(int width, int height)
        {
            return _algorithm.Create(width, height);
        }
    }
}