using System;
using UnityEngine;

namespace BlissfulMaze.Common.Player
{
    public interface IPlayerInputService
    {
        event Action<Vector3> OnLeft;
        event Action<Vector3> OnRight;
        event Action<Vector3> OnDown;
        event Action<Vector3> OnUp;
        bool IsEnabled { get; set; }
    }
}