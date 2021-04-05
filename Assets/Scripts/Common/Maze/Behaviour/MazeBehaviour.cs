using UnityEngine;
using Zenject;

namespace BlissfulMaze.Common
{
    public class MazeBehaviour : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject MazeCellPrefab;
        [Header("Size")]
        public int Width;
        public int Height;

        private IMazeInstantiator _mazeInstantiator;
        private IMazeGenerator _mazeGenerator;

        [Inject]
        private void Construct(IMazeInstantiator mazeInstantiator, IMazeGenerator mazeGenerator)
        {
            _mazeInstantiator = mazeInstantiator;
            _mazeGenerator = mazeGenerator;
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