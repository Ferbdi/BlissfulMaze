using UnityEngine;

namespace BlissfulMaze.Common.Player
{
    public interface IPlayer
    {
        void Tumble(Vector3 direction);
    }
}