namespace BlissfulMaze.Common.Maze
{
    public interface IMaze
    {
        TypeMazeCell[,] Cells { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }
}