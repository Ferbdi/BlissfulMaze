using UnityEngine;
using Zenject;
using BlissfulMaze.Common.Player;
using BlissfulMaze.Common.Maze;

namespace BlissfulMaze.Infrastructure
{
    public class GameLogicService : IInitializable, IGameLogicService
    {
        private Player _player;
        private IPlayerInputService _playerInputService;
        private MazeBehaviour _mazeBehaviour;

        [Inject]
        private void Construct(Player player, IPlayerInputService playerInputService, MazeBehaviour mazeBehaviour)
        {
            _player = player;
            _playerInputService = playerInputService;
            _mazeBehaviour = mazeBehaviour;
        }

        public void Initialize()
        {
            _mazeBehaviour.FinishTrigger.OnEnter += OnFinishTriggerEnter;
        }

        private async void OnFinishTriggerEnter(Collider collider)
        {
            try
            {
                if (collider.gameObject == _player.gameObject)
                {
                    _playerInputService.IsEnabled = false;
                    await _mazeBehaviour.Recreate();
                    _playerInputService.IsEnabled = true;
                }
            }
            catch
            {
                Debug.LogWarning("[WARNING] MazeBehaviour is missing. Maybe scene is reloaded.");
            }
        }
    }
}