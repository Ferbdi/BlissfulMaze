using UnityEngine;
using Zenject;

namespace BlissfulMaze.Common.Maze
{
    public class MazeBehaviour : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject MazeCellPrefab;
        [Header("Size")]
        public int Width;
        public int Height;

        private IMazeGenerator _mazeGenerator;
        private IMazeInstantiator _mazeInstantiator;

        [Inject]
        private void Construct(IMazeGenerator mazeGenerator, IMazeInstantiator mazeInstantiator)
        {
            _mazeGenerator = mazeGenerator;
            _mazeInstantiator = mazeInstantiator;
        }

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            var maze = _mazeGenerator.Generate(Width, Height);
            _mazeInstantiator.InstantiateRoutine(maze, MazeCellPrefab, transform);
        }
    }
}