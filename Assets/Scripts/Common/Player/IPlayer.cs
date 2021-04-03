using UnityEngine;

namespace BlissfulMaze.Common
{
    public interface IPlayer
    {
        void Tumble(Vector3 direction);
    }
}