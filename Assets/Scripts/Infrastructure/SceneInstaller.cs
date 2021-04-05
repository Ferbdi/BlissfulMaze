using Zenject;
using BlissfulMaze.Common;
using BlissfulMaze.Common.Maze;
using UnityEngine;

namespace BlissfulMaze.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Vector3 _spawnPosition;

        public override void InstallBindings()
        {
            BindPlayerInputService();
            BindPlayerFactory();
            BindPlayerSpawner();

            Container
                .BindInterfacesTo<MazeGenerator>()
                .AsSingle();
            Container
                .BindInterfacesTo<SimpleMazeGenerationAlgorithm>()
                .AsSingle();
            Container
                .BindInterfacesTo<MazeInstantiator>()
                .AsSingle();
        }

        private void BindPlayerFactory()
        {
            Container
                .BindFactory<Vector3, Player, Player.Factory>()
                .FromComponentInNewPrefab(_playerPrefab);
        }

        private void BindPlayerSpawner()
        {
            Container
                .BindInterfacesTo<PlayerSpawner>()
                .AsSingle()
                .WithArguments(_spawnPosition);
        }

        private void BindPlayerInputService()
        {
            Container
                .Bind(typeof(ITickable), typeof(IPlayerInputService))
                .To<PlayerInputService>()
                .AsSingle();
        }
    }
}