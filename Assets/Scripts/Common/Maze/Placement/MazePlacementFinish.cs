using UnityEngine;
using System.Collections;
using Zenject;
using System.Threading.Tasks;

namespace BlissfulMaze.Common.Maze
{
    public class MazePlacementFinish : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private async Task Despawn()
        {
            var main = _particleSystem.main;
            var gravityBefore = main.gravityModifier;
            main.gravityModifier = 2f;
            _particleSystem.Stop();

            await Task.Delay(1000);
            main.gravityModifier = gravityBefore;
        }

        public class Pool : MonoMemoryPool<Vector3, MazePlacementFinish>
        {
            protected override void Reinitialize(Vector3 position, MazePlacementFinish mazePlacementFinish)
            {
                mazePlacementFinish.transform.position = position;
            }

            protected override async void OnDespawned(MazePlacementFinish item)
            {
                await item.Despawn();
                base.OnDespawned(item);
            }
        }
    }
}