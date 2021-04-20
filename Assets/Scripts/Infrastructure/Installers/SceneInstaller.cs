using Zenject;
using UnityEngine;
using BlissfulMaze.Common.Maze;
using BlissfulMaze.Common.Player;

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
            BingGameLogicService();
            BindMazeBehaviour();
            BindPlayerInputService();
            BindPlayerFactory();
            BindPlayer();
        }

        private void BindMazeBehaviour()
        {
            Container
                .Bind<MazeBehaviour>()
                .FromInstance(_mazeBehaviour)
                .AsSingle();
        }

        private void BingGameLogicService()
        {
            Container
                .BindInterfacesTo<GameLogicService>()
                .AsSingle();
        }

        private void BindPlayer()
        {
            Container
                .Bind<Player>()
                .FromMethod(() => Container.Resolve<Player.Factory>().Create(_spawnPosition))
                .AsSingle()
                .NonLazy();
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