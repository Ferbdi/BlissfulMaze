namespace BlissfulMaze.Common.Maze
{
    public class Maze : IMaze
    {
        public TypeMazeCell[,] Cells { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}