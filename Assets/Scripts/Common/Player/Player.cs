using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BlissfulMaze.Common
{
    public class Player : MonoBehaviour, IPlayer
    {
        public LayerMask ObstacleMask;

        [SerializeField] private float _tumblingDuration = 0.2f;
        private bool _isTumbling;

        private IPlayerInputService _playerInputService;

        [Inject]
        private void Construct(IPlayerInputService playerInputService, Vector3 spawnPosition)
        {
            _playerInputService = playerInputService;
            transform.position = spawnPosition;
        }

        private void Awake()
        {
            _playerInputService.OnDown += Tumble;
            _playerInputService.OnUp += Tumble;
            _playerInputService.OnRight += Tumble;
            _playerInputService.OnLeft += Tumble;
        }

        private void OnDisable()
        {
            _playerInputService.OnDown -= Tumble;
            _playerInputService.OnUp -= Tumble;
            _playerInputService.OnRight -= Tumble;
            _playerInputService.OnLeft -= Tumble;
        }

        public bool IsCanTumble(Vector3 direction)
        {
            var ray = new Ray(transform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
            return !Physics.Raycast(ray, 1, ObstacleMask);
        }

        public void Tumble(Vector3 direction)
        {
            if (_isTumbling) return;
            if (IsCanTumble(direction))
                StartCoroutine(TumbleRoutine(direction));
        }

        private IEnumerator TumbleRoutine(Vector3 direction)
        {
            _isTumbling = true;

            var rotAxis = Vector3.Cross(Vector3.up, direction);
            var pivot = (transform.position + Vector3.down * 0.5f) + direction * 0.5f;

            var startRotation = transform.rotation;
            var endRotation = Quaternion.AngleAxis(90.0f, rotAxis) * startRotation;

            var startPosition = transform.position;
            var endPosition = transform.position + direction;

            var rotSpeed = 90.0f / _tumblingDuration;
            var t = 0.0f;

            while (t < _tumblingDuration)
            {
                t += Time.deltaTime;
                if (t < _tumblingDuration)
                {
                    transform.RotateAround(pivot, rotAxis, rotSpeed * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    transform.rotation = endRotation;
                    transform.position = endPosition;
                }
            }

            _isTumbling = false;
        }

        public class Factory : PlaceholderFactory<Vector3, Player> { }
    }
}