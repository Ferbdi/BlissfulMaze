using UnityEngine;
using Zenject;

namespace BlissfulMaze.Entities
{
    public class PlayerSpawner : IInitializable
    {
        private readonly Player.Factory _playerFactory;

        public PlayerSpawner(Player.Factory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public void Initialize()
        {
            _playerFactory.Create(new Vector3(0, 1, 0));
        }
    }
}