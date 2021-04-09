using UnityEngine;
using System;
using System.Collections.Generic;

namespace BlissfulMaze.Common
{
    [RequireComponent(typeof(Collider))]
    public class TriggerHandler : MonoBehaviour, ITriggerHandler
    {
        public event Action<Collider> Enter;
        public event Action<Collider> Stay;
        public event Action<Collider> Exit;

        private List<GameObject> _objectsInside = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            _objectsInside.Add(other.gameObject);
            Enter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_objectsInside.Contains(other.gameObject))
                _objectsInside.Add(other.gameObject);
            Stay?.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _objectsInside.Remove(other.gameObject);
            Exit?.Invoke(other);
        }

        public bool HasInside(GameObject go) => _objectsInside.Contains(go);
    }
}