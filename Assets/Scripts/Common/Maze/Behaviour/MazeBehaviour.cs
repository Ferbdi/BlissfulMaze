using UnityEngine;
using Zenject;
using System.Threading.Tasks;

namespace BlissfulMaze.Common.Maze
{
    public class MazeBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private MazePlacementSettings _mazePlacementSettings;
        [Header("Size")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public MazePlacementSettings MazePlacementSettings { get => _mazePlacementSettings; }
        public ITriggerHandler FinishTrigger { get => _mazePlacementService.FinishTrigger; }
        private IMazeGenerator _mazeGenerator;
        private IMazePlacementService _mazePlacementService;
        private bool _isRecreating;
        private bool _finishInStartPosition;

        [Inject]
        private void Construct(IMazeGenerator mazeGenerator, IMazePlacementService mazePlacementService)
        {
            _mazeGenerator = mazeGenerator;
            _mazePlacementService = mazePlacementService;
        }

        private void Awake()
        {
            Setup();
            _mazePlacementService.MoveUp();
        }

        private void Setup()
        {
            var maze = _mazeGenerator.Generate(_width, _height, _finishInStartPosition ? new Vector2Int(1, 1) : new Vector2Int(_width - 2, _height - 2));
            _finishInStartPosition = !_finishInStartPosition;
            _mazePlacementService.Setup(maze, _mazePlacementSettings, transform);
        }

        public async Task Recreate()
        {
            if (_isRecreating) return;
            _isRecreating = true;

            while (_mazePlacementService.IsMoving) { await Task.Yield(); }
            _mazePlacementService.MoveDown();
            while (_mazePlacementService.IsMoving) { await Task.Yield(); }
            Setup();
            _mazePlacementService.MoveUp();
            while (_mazePlacementService.IsMoving) { await Task.Yield(); }

            _isRecreating = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_mazePlacementService.PlacementState == PlacementState.Up && !_mazePlacementService.IsMoving)
                    _mazePlacementService.MoveDown();
                else if (_mazePlacementService.PlacementState == PlacementState.Down && !_mazePlacementService.IsMoving)
                {
                    Setup();
                    _mazePlacementService.MoveUp();
                }
            }
        }
    }
}