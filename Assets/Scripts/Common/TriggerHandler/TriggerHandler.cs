using UnityEngine;
using System;
using System.Collections.Generic;

namespace BlissfulMaze.Common
{
    [RequireComponent(typeof(Collider))]
    public class TriggerHandler : MonoBehaviour, ITriggerHandler
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnStay;
        public event Action<Collider> OnExit;

        private List<GameObject> _objectsInside = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            _objectsInside.Add(other.gameObject);
            OnEnter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_objectsInside.Contains(other.gameObject))
                _objectsInside.Add(other.gameObject);
            OnStay?.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _objectsInside.Remove(other.gameObject);
            OnExit?.Invoke(other);
        }

        public bool HasInside(GameObject go) => _objectsInside.Contains(go);
    }
}