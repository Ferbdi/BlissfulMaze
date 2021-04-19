using System;
using UnityEngine;
using Zenject;

namespace BlissfulMaze.Common
{
    public class PlayerInputService : ITickable, IPlayerInputService
    {
        public event Action<Vector3> OnLeft;
        public event Action<Vector3> OnRight;
        public event Action<Vector3> OnDown;
        public event Action<Vector3> OnUp;
        public bool IsEnabled { get; set; } = true;

        private void InputCheck()
        {
            if (Input.GetKey(KeyCode.A))
                OnLeft?.Invoke(Vector3.left);
            if (Input.GetKey(KeyCode.D))
                OnRight?.Invoke(Vector3.right);
            if (Input.GetKey(KeyCode.S))
                OnDown?.Invoke(Vector3.back);
            if (Input.GetKey(KeyCode.W))
                OnUp?.Invoke(Vector3.forward);
        }

        public void Tick()
        {
            if (IsEnabled)
                InputCheck();
        }
    }
}