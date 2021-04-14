using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace BlissfulMaze.Common.Maze
{
    public class MazeBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private MazePlacementSettings _mazePlacementSettings;
        [Header("Size")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public ITriggerHandler FinishTrigger { get => _mazePlacementService.FinishTrigger; }
        private IMazeGenerator _mazeGenerator;
        private IMazePlacementService _mazePlacementService;

        [Inject]
        private void Construct(IMazeGenerator mazeGenerator, IMazePlacementService mazePlacementService)
        {
            _mazeGenerator = mazeGenerator;
            _mazePlacementService = mazePlacementService;
        }

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            var maze = _mazeGenerator.Generate(_width, _height);
            _mazePlacementService.Instantiate(maze, _mazePlacementSettings, transform);
            //_mazePlacementService.MoveUp(cells, _mazePlacementSettings);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (_mazePlacementService.PlacementState == PlacementState.Up)
                    _mazePlacementService.MoveDown(_mazePlacementSettings);
                else
                    _mazePlacementService.MoveUp(_mazePlacementSettings);
            }
        }
    }
}