using UnityEngine;

namespace BlissfulMaze.Common.Maze
{
    [CreateAssetMenu(fileName = "Maze Placement Settings", menuName = "ScriptableObjects/Maze/Placement Settings", order = 0)]
    public class MazePlacementSettings : ScriptableObject
    {
        [SerializeField] private GameObject _mazeCellPrefab;
        [SerializeField] private GameObject _mazeFinishTriggerPrefab;
        [SerializeField] private AnimationCurve _placementCurve;
        [SerializeField] private float _speedOfPlacementUp;

        public GameObject MazeCellPrefab => _mazeCellPrefab;
        public GameObject MazeFinishTriggerPrefab => _mazeFinishTriggerPrefab;
        public AnimationCurve PlacementCurve => _placementCurve;
        public float SpeedOfPlacementUp => _speedOfPlacementUp;
    }
}