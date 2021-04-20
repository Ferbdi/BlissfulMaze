using UnityEngine;
using System;

namespace BlissfulMaze.Common
{
    public interface ITriggerHandler
    {
        event Action<Collider> OnEnter;
        event Action<Collider> OnStay;
        event Action<Collider> OnExit;
        bool HasInside(GameObject go);
    }
}