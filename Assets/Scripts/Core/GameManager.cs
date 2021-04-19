using UnityEngine;
using Zenject;
using BlissfulMaze.Common;
using BlissfulMaze.Common.Maze;
using UnityEngine.SceneManagement;

namespace BlissfulMaze.Core
{
    public class GameManager : MonoBehaviour, IGameManager
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

        private void Start()
        {
            _mazeBehaviour.FinishTrigger.Enter += OnFinishTriggerEnter;
        }

        private async void OnFinishTriggerEnter(Collider collider)
        {
            try
            {
                if (collider.gameObject == _player.gameObject)
                {
                    Debug.Log("Finish!!!");
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}