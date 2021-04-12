using Zenject;
using BlissfulMaze.Core;
using BlissfulMaze.Common;
using BlissfulMaze.Common.Maze;
using UnityEngine;

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

            BindMazeGenerator();
            BindMazeGenerationAlgorithm();
            BindMazePlacementService();
            BindMazeBehaviour();
        }

        private void BindMazeBehaviour()
        {
            Container
                .Bind<MazeBehaviour>()
                .FromInstance(_mazeBehaviour)
                .AsSingle();
        }

        private void BindMazePlacementService()
        {
            Container
                .BindInterfacesTo<MazePlacementService>()
                .AsSingle();
        }

        private void BindMazeGenerationAlgorithm()
        {
            Container
                .BindInterfacesTo<SimpleMazeGenerationAlgorithm>()
                .AsSingle();
        }

        private void BindMazeGenerator()
        {
            Container
                .BindInterfacesTo<MazeGenerator>()
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