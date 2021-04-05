namespace BlissfulMaze.Common
{
    public interface IMaze
    {
        TypeMazeCell[,] Cells { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }
}