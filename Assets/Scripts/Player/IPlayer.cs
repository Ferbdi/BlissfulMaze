using UnityEngine;

namespace BlissfulMaze.Entities
{
    public interface IPlayer
    {
        void Tumble(Vector3 direction);
    }
}