using UnityEngine;
using Zenject;

namespace BlissfulMaze.Common
{
    public class PlayerSpawner : IInitializable
    {
        private readonly Player.Factory _playerFactory;
        private readonly Vector3 _spawnPosition;

        public PlayerSpawner(Player.Factory playerFactory, Vector3 spawnPosition)
        {
            _playerFactory = playerFactory;
            _spawnPosition = spawnPosition;
        }

        public void Initialize()
        {
            _playerFactory.Create(_spawnPosition);
        }
    }
}