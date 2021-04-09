using UnityEngine;
using System;

namespace BlissfulMaze.Common
{
    public interface ITriggerHandler
    {
        event Action<Collider> Enter;
        event Action<Collider> Stay;
        event Action<Collider> Exit;
        bool HasInside(GameObject go);
    }
}