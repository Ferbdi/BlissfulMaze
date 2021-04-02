using Zenject;
using BlissfulMaze.Core;
using BlissfulMaze.Entities;
using UnityEngine;

namespace BlissfulMaze.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        public GameObject playerPrefab;

        public override void InstallBindings()
        {
            BindPlayerInputService();
            BindPlayerSpawner();
            BindPlayerFactory();
        }

        private void BindPlayerFactory()
        {
            Container
                .BindFactory<Vector3, Player, Player.Factory>()
                .FromComponentInNewPrefab(playerPrefab);
        }

        private void BindPlayerSpawner()
        {
            Container
                .BindInterfacesTo<PlayerSpawner>()
                .AsSingle();
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