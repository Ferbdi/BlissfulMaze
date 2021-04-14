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
        private MazeBehaviour _mazeBehaviour;

        [Inject]
        private void Construct(Player player, MazeBehaviour mazeBehaviour)
        {
            _player = player;
            _mazeBehaviour = mazeBehaviour;
        }

        private void Start()
        {
            _mazeBehaviour.FinishTrigger.Enter += (Collider collider) =>
            {
                if (collider.gameObject == _player.gameObject)
                    Debug.Log("Finish!!!");
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}