using Zenject;
using BlissfulMaze.Common.Maze;
using UnityEngine;

namespace BlissfulMaze.Infrastructure
{
    public class MazeInstaller : MonoInstaller
    {
        [SerializeField] private MazeBehaviour _mazeBehaviour;

        public override void InstallBindings()
        {
            BindMazeGenerator();
            BindMazeGenerationAlgorithm();
            BindMazePlacementService();
            BindMazePlacementCellPool();
            BindMazePlacementFinish();
        }

        private void BindMazePlacementFinish()
        {
            Container
               .BindMemoryPool<MazePlacementFinish, MazePlacementFinish.Pool>()
               .WithInitialSize(1)
               .FromComponentInNewPrefab(_mazeBehaviour.MazePlacementSettings.MazeFinishTriggerPrefab)
               .UnderTransform(_mazeBehaviour.transform);
        }

        private void BindMazePlacementCellPool()
        {
            Container
                .BindMemoryPool<MazePlacementCell, MazePlacementCell.Pool>()
                .WithInitialSize(100)
                .FromComponentInNewPrefab(_mazeBehaviour.MazePlacementSettings.MazeCellPrefab)
                .UnderTransform(_mazeBehaviour.transform);
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
    }
}