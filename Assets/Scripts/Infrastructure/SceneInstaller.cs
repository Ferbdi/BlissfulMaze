using Zenject;
using BlissfulMaze.Core;
using BlissfulMaze.Common;
using UnityEngine;
using BlissfulMaze.Common.Maze;

namespace BlissfulMaze.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Vector3 _spawnPosition;
        [Header("Maze")]
        [SerializeField] private MazeBehaviour _mazeBehaviour;

        public override void InstallBindings()
        {
            BindPlayerInputService();
            BindPlayerFactory();
            BindPlayer();

            Container
               .Bind<MazeBehaviour>()
               .FromInstance(_mazeBehaviour)
               .AsSingle();
        } 

        private void BindPlayer()
        {
            Container
                .Bind<Player>()
                .FromMethod(() => Container.Resolve<Player.Factory>().Create(_spawnPosition))
                .AsSingle();
        }

        private void BindPlayerFactory()
        {
            Container
                .BindFactory<Vector3, Player, Player.Factory>()
                .FromComponentInNewPrefab(_playerPrefab);
        }

        private void BindPlayerInputService()
        {
            Container
                .BindInterfacesTo<PlayerInputService>()
                .AsSingle();
        }
    }
}