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

        private IMazeGenerator _mazeGenerator;
        private IMazePlacementService _mazePlacementService;
        private bool _isRecreating;
        private bool _finishInStartPosition;

        public MazePlacementSettings MazePlacementSettings => _mazePlacementSettings;
        public ITriggerHandler FinishTrigger => _mazePlacementService.FinishTrigger;

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
            var finishPosition = _finishInStartPosition ? new Vector2Int(1, 1) : new Vector2Int(_width - 2, _height - 2);
            _finishInStartPosition = !_finishInStartPosition;

            var maze = _mazeGenerator.Generate(_width, _height, finishPosition);
            _mazePlacementService.Setup(maze, _mazePlacementSettings, transform);
        }

        private async Task WaitForItMoving(IMazePlacementService mazePlacementService)
        {
            while (mazePlacementService.IsMoving)
                await Task.Yield();
        }

        public async Task Recreate()
        {
            if (_isRecreating) return;
            _isRecreating = true;

            await WaitForItMoving(_mazePlacementService);
            _mazePlacementService.MoveDown();
            await WaitForItMoving(_mazePlacementService);
            Setup();
            _mazePlacementService.MoveUp();
            await WaitForItMoving(_mazePlacementService);

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